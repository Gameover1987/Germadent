using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace Germadent.Rma.App.Reporting.TemplateProcessing
{
    public static class WordXmlPartAssembler
    {

        private static readonly XName[] MetaToXmlBlocks =  {
            MetaNames.Conditional,
            MetaNames.EndConditional,
            MetaNames.Repeat,
            MetaNames.EndRepeat,
            MetaNames.Table
        };

        private static readonly XName[] Metas = {
            MetaNames.Content,
            MetaNames.Repeat,
            MetaNames.Table,
            MetaNames.Conditional,
            MetaNames.Counter
        };


        private static readonly Dictionary<XName, XName> MatchingEndNames = new Dictionary<XName, XName>()
                                                                    {
                                                                        {MetaNames.Repeat, MetaNames.EndRepeat},
                                                                        {MetaNames.Conditional, MetaNames.EndConditional},
                                                                    };


        public static XElement Assemble(XElement xmlRoot, JToken jsonData)
        {
            var xDocRoot = RemoveGoBackBookmarks(xmlRoot);

            //// content controls in cells can surround the W.Tc element, so transform so that such content controls are within the cell content
            xDocRoot = (XElement)NormalizeContentControlsInCells(xDocRoot);


            xDocRoot = (XElement)TransformToMetadata(xDocRoot);

            //// Table might have been placed at run-level, when it should be at block-level, so fix this.
            //// Repeat, EndRepeat, Conditional, EndConditional are allowed at run level, but only if there is a matching pair
            //// if there is only one Repeat, EndRepeat, Conditional, EndConditional, then move to block level.
            //// if there is a matching pair, then is OK.
            xDocRoot = (XElement)ForceBlockLevelAsAppropriate(xDocRoot);

            ////
            xDocRoot = WrapMetadataBlocks(xDocRoot);


            //// any EndRepeat, EndConditional that remain are orphans, so replace with an error
            ////TODOD кажется уже видел подобную проверку на предыдущем уровне
            xDocRoot = ProcessOrphanMetaBlocks(xDocRoot);

            //// do the actual content replacement
            xDocRoot = (XElement)ContentReplacementTransform(xDocRoot, jsonData);


            return xDocRoot;
        }



        
        
        public static XElement RemoveGoBackBookmarks(XElement xElement)
        {
            var clonedDoc = XElementFactory.Clone(xElement);

            var bms = MakeEmptyList<XElement>();

            bms.AddRange(
                clonedDoc.GetBookmarks(Wrd.BookmarkStart, Wrd.Name, "_GoBack"));
            var bmStartsIds = bms.GetElementsAttribute(Wrd.Id);
            bms.AddRange(
                clonedDoc.GetBookmarks(Wrd.BookmarkEnd, Wrd.Id, bmStartsIds));

            bms.DropFromTree();
            
            return clonedDoc;
        }


       
        public static object NormalizeContentControlsInCells(XNode node)
        {
            var element = node as XElement;
            if (element == null)
                return node;

            
            if (element.Name == Wrd.Sdt && element.ParentHasXName(Wrd.Tr))
            {

                return XElementFactory.Make(Wrd.Tc,
                    XElementFactory.Make(Wrd.Sdt,
                        element.Elements(Wrd.SdtPr),
                        element.Elements(Wrd.SdtEndPr),
                        XElementFactory.Make(Wrd.SdtContent,
                            element.Elements(Wrd.SdtContent).Elements(Wrd.Tc).GetElementsWithNamesExcept(Wrd.TcPr))));

            }
            return XElementFactory.MakeRecursively(element, NormalizeContentControlsInCells);
            
            
        }



        
        public static object TransformToMetadata(XNode node)
        {

            var element = node as XElement;
            if (element == null)
                return node;


            if (element.Name != Wrd.Sdt)
                return XElementFactory.MakeRecursively(element, TransformToMetadata);


            var textBlocks = element.Descendants(Wrd.T).Select(t => (string)t);
            var xmlText = ConvertToXmlFormat(textBlocks);


            if (!xmlText.StartsWith("<"))
                return XElementFactory.MakeRecursively(element, TransformToMetadata);

          
            var ccContent = element.Elements(Wrd.SdtContent).Elements();
            return TransformXmlTextToMetadata(xmlText, ccContent, element.ParentHasXName(Wrd.P));
        }

        
        public static object ForceBlockLevelAsAppropriate(XNode node)
        {

            var element = node as XElement;
            if (element == null)
                return node;


            if (element.Name != Wrd.P)
                return XElementFactory.MakeRecursively(element, ForceBlockLevelAsAppropriate);


            var childMeta = GetChildMetaTags(element);

            if (childMeta.Count() == 1)
            {
                var otherTextInParagraph = element.Elements(Wrd.R).Elements(Wrd.T).Select(t => (string)t).ToList();
                var agregatedText = StringConcatenate(otherTextInParagraph).Trim();

                return agregatedText != "" ? 
                        XElementFactory.CreateInvalidMetaTagMarkdownErrorMessage(agregatedText) : 
                        MetaLevelUp(childMeta.First());

            }

            return MetaTagMismatched(childMeta.ToArray()) ? 
                XElementFactory.CreateParaErrorMessage("Error: Не все блоки метаданных имеют парные закрывающие теги на уровне Runa") 
                : XElementFactory.MakeRecursively(element, ForceBlockLevelAsAppropriate);
        }




        public static XElement WrapMetadataBlocks(XElement xDocument)
        {

            var xDoc = XElementFactory.Clone(xDocument);

            var tables = xDoc.Descendants(MetaNames.Table).ToList();
            foreach (var table in tables)
            {
                WrapTable(table);
            }

            SetMetablocksDepthAtrribute(xDoc);


            var metablocks = xDoc.Descendants().Where(d => (d.Name == MetaNames.Repeat || d.Name == MetaNames.Conditional)).ToList();
            foreach (var metablock in metablocks)
            {
                WrapPairedMetaBlocks(metablock);
            }


            return xDoc;

        }




        public static XElement ProcessOrphanMetaBlocks(XElement xDoc)
        {
            var xDocument = XElementFactory.Clone(xDoc);

            foreach (var element in xDocument.Descendants(MetaNames.EndRepeat).ToList())
            {
                var error = XElementFactory.CreateParaErrorMessage("Error: EndRepeat without matching Repeat");
                element.ReplaceWith(error);
            }
            foreach (var element in xDocument.Descendants(MetaNames.EndConditional).ToList())
            {
                var error = XElementFactory.CreateParaErrorMessage("Error: EndConditional without matching Conditional");
                element.ReplaceWith(error);
            }

            return xDocument;
        }


        
        public static object ContentReplacementTransform(XNode node, JToken data)
        {

            var element = node as XElement;
            if (element == null)
                return node;

            if (Metas.Contains(element.Name))
            {
                var jPath = (string)element.Attribute(MetaNames.Select);
                if (jPath == null)
                    return XElementFactory.CreateContextErrorMessage(element, "Meta tag hasn't select attribute");

                var selectedToken = data.SelectToken(jPath);
                if (selectedToken == null)
                    return XElementFactory.CreateContextErrorMessage(element, 
                        $"JPathException: JPath expression ({jPath}) returned no results");

                
                if (element.Name == MetaNames.Content)
                {
                    return AssembleMetaContent(element, selectedToken);
                }
                if (element.Name == MetaNames.Counter)
                {
                    return AssembleMetaCounter(element, selectedToken);
                }
                if (element.Name == MetaNames.Repeat)
                {
                    return AssembleMetaRepeat(element, selectedToken);
                }
                if (element.Name == MetaNames.Table)
                {
                    return AssembleMetaTable(element, selectedToken);
                }
                if (element.Name == MetaNames.Conditional)
                {
                    return AssembleMetaConditional(element, selectedToken, data);
                }
            }

            
            return XElementFactory.MakeRecursively(element, data, ContentReplacementTransform);


        }




        
        private static object AssembleSingleDataMeta(XElement element, string contentText)
        {
            var para = element.Descendants(Wrd.P).FirstOrDefault();
            var run = element.Descendants(Wrd.R).FirstOrDefault();


            if (para != null)
            {
                var newPara = XElementFactory.Make(Wrd.P, para.Elements(Wrd.PPr));
                newPara.Add(XElementFactory.Make(Wrd.R,
                    para.Elements(Wrd.R).Elements(Wrd.RPr).FirstOrDefault(),
                    XElementFactory.Make(Wrd.T, contentText)));
                return newPara;
            }

            XElement newRun;
            if (run != null)
            {
                newRun = XElementFactory.Make(Wrd.R,
                    run.Elements(Wrd.RPr),
                    XElementFactory.Make(Wrd.T, contentText));
                return newRun;
            }

            newRun = XElementFactory.Make(Wrd.R,
                    XElementFactory.Make(Wrd.T, contentText));
            return newRun;
        }





        private static object AssembleMetaContent(XElement element, JToken data)
        {            
            var contentText = data.ToString();
            var defaultValue = (string)element.Attribute(MetaNames.DefaultValue);

            if (string.IsNullOrEmpty(contentText) && defaultValue != null)
                contentText = defaultValue;
            contentText = Formatting(element, contentText);
            return AssembleSingleDataMeta(element, contentText);
        }

        private static string Formatting(XElement element, string data)
        {
            var dateFormat = (string)element.Attribute(MetaNames.DateFormat);

            if (!string.IsNullOrEmpty(dateFormat))
                data = DateTime.Parse(data).ToString(dateFormat);
            return data;
        }



        private static object AssembleMetaCounter(XElement element, JToken data)
        {
            if (data.Type != JTokenType.Array)
            {
                return XElementFactory.CreateContextErrorMessage(element,
                    $"JPath expression expected to return array of data, but returns ({data.Type})");
            }

            
            var contentText = $"{data.Count()}";

            return AssembleSingleDataMeta(element, contentText);

        }



        private static object AssembleMetaRepeat(XElement element, JToken data)
        {

            if (data.Type != JTokenType.Array)
            {
                return XElementFactory.CreateContextErrorMessage(element,
                    $"JPath expression expected to return array of data, but returns ({data.Type})");
            }


            if (!data.Any())
            {
                var para = element.Descendants(Wrd.P).FirstOrDefault();
                var emptyElement = para == null
                                    ? XElementFactory.MakeEmptyRun()
                                    : XElementFactory.MakeEmptyPara();
                return emptyElement;
            }

            var newContent = data.Select(d =>
            {
                var content = element
                    .Elements()
                    .Select(e => ContentReplacementTransform(e, d.Value<JToken>()))
                    .ToList();
                return content;
            }).ToList();

            return newContent;
        }

        private static object AssembleMetaTable(XElement element, JToken data)
        {

            if (data.Type != JTokenType.Array)
            {
                return XElementFactory.CreateContextErrorMessage(element, 
                    $"JPath expression expected to return array of data, but returns ({data.Type})");
            }


            var table = element.Element(Wrd.Tbl);
            if (table == null)
                return XElementFactory.CreateContextErrorMessage(element, "Metatable tag doesn't contains child table tag");

            var protoRow = table.Elements(Wrd.Tr).Skip(1).FirstOrDefault();

            if (protoRow == null)
                return XElementFactory.CreateContextErrorMessage(element, "Table does not contain a prototype row");

            protoRow.Descendants(Wrd.BookmarkStart).Remove();
            protoRow.Descendants(Wrd.BookmarkEnd).Remove();

            var newTable = XElementFactory.Make(Wrd.Tbl,
                table.Elements(Wrd.TblPr),
                table.Elements(Wrd.TblGrid),
                table.Elements(Wrd.Tr).First(),
                data.Select(d =>
                    XElementFactory.Make(Wrd.Tr,
                        protoRow.Elements(Wrd.TrPr),
                        protoRow.Elements(Wrd.Tc)
                            .Select(tc =>
                                XElementFactory.Make(Wrd.Tc,
                                    tc.Nodes().Select(n => ContentReplacementTransform(n, d.Value<JToken>()))
                                            ))))
                            );
            return newTable;
        }


        private static object AssembleMetaConditional(XElement element, JToken condData, JToken data )
        {
            var condValue = condData.ToString();

            var match = (string)element.Attribute(MetaNames.Match);
            var notMatch = (string)element.Attribute(MetaNames.NotMatch);

            if (match == null && notMatch == null)
                return XElementFactory.CreateContextErrorMessage(element, "Conditional: Must specify either Match or NotMatch");
            if (match != null && notMatch != null)
                return XElementFactory.CreateContextErrorMessage(element, "Conditional: Cannot specify both Match and NotMatch");


            if ((match != null && condValue == match) || (notMatch != null && condValue != notMatch))
            {
                var content = element.Elements().Select(e => ContentReplacementTransform(e, data));
                return content;
            }
            return null;
        }


        
        




        private static void WrapTable(XElement table)
        {
            var followingElement = table.ElementsAfterSelf().FirstOrDefault();
            if (followingElement == null || followingElement.Name != Wrd.Tbl)
            {
                table.ReplaceWith(XElementFactory.CreateParaErrorMessage("Table metadata is not immediately followed by a table"));
                return;
            }
            table.RemoveNodes();
            // detach w:tbl from parent, and add to Table metadata
            followingElement.Remove();
            table.Add(followingElement);
        }


        private static void WrapPairedMetaBlocks(XElement metadata)
        {
            
            var depth = (int)metadata.Attribute(MetaNames.Depth);
            var matchingEndName = MatchingEndNames[metadata.Name];


            var matchingEnd = metadata.ElementsAfterSelf(matchingEndName).FirstOrDefault(end => (int)end.Attribute(MetaNames.Depth) == depth);
            if (matchingEnd == null)
            {
                //если metablock находится на уровне Runов -? По идее, получим вложенные праграфы
                metadata.ReplaceWith(XElementFactory.CreateParaErrorMessage($"{metadata.Name.LocalName} does not have matching {matchingEndName.LocalName}"));
                return;
            }
            metadata.RemoveNodes();
            var contentBetween = metadata.ElementsAfterSelf().TakeWhile(after => after != matchingEnd).ToList();
            foreach (var item in contentBetween)
                item.Remove();
            contentBetween = contentBetween.Where(n => n.Name != Wrd.BookmarkStart && n.Name != Wrd.BookmarkEnd).ToList();
            metadata.Add(contentBetween);
            metadata.Attributes(MetaNames.Depth).Remove();
            matchingEnd.Remove();
        }


        private static void SetMetablocksDepthAtrribute(XElement xDoc)
        {
            var repeatDepth = 0;
            var conditionalDepth = 0;
            foreach (var metadata in xDoc.Descendants().Where(d =>
                    d.Name == MetaNames.Repeat ||
                    d.Name == MetaNames.Conditional ||
                    d.Name == MetaNames.EndRepeat ||
                    d.Name == MetaNames.EndConditional))
            {
                if (metadata.Name == MetaNames.Repeat)
                {
                    ++repeatDepth;
                    metadata.Add(XElementFactory.MakeAttribute(MetaNames.Depth, repeatDepth.ToString()));
                    continue;
                }
                if (metadata.Name == MetaNames.EndRepeat)
                {
                    metadata.Add(XElementFactory.MakeAttribute(MetaNames.Depth, repeatDepth.ToString()));
                    --repeatDepth;
                    continue;
                }
                if (metadata.Name == MetaNames.Conditional)
                {
                    ++conditionalDepth;
                    metadata.Add(XElementFactory.MakeAttribute(MetaNames.Depth, conditionalDepth.ToString()));
                    continue;
                }
                if (metadata.Name == MetaNames.EndConditional)
                {
                    metadata.Add(XElementFactory.MakeAttribute(MetaNames.Depth, conditionalDepth.ToString()));
                    --conditionalDepth;
                }
            }
        }



        
        private static bool MetaTagMismatched(XElement[] childMeta)
        {
            if (childMeta.Count()%2 != 0)
                return true;

            if (childMeta.Count(c => c.Name == MetaNames.Repeat) !=
                childMeta.Count(c => c.Name == MetaNames.EndRepeat))
                return true;

            if (childMeta.Count(c => c.Name == MetaNames.Conditional) !=
                childMeta.Count(c => c.Name == MetaNames.EndConditional))
                return true;
            
            return false;
        }

        
        private static XElement MetaLevelUp(XElement meta)
        {
            //return XElementFactory.Make(meta.MaterialName,
            //        meta.Attributes(),
            //        XElementFactory.Make(W.P,
            //                para.Attributes(),
            //                para.Elements(W.PPr),
            //                meta.Elements()));

            //TODO от мета-тега нам нужно только его имя и атрибуты. Поэтому копировать его внутренности смысла нет.
            //В оригинале они удалялись на следующем шаге.
            return XElementFactory.Make(meta.Name, meta.Attributes());

        }


        private static XElement[] GetChildMetaTags(XElement element)
        {
            return element.Elements().Where(n => MetaToXmlBlocks.Contains(n.Name)).ToArray();
        }

        


        private static string ConvertToXmlFormat(IEnumerable<string> textBlocks)
        {
            return StringConcatenate(textBlocks)
                        .Trim()
                        .Replace('“', '"')
                        .Replace('”', '"');
        }

        private static string StringConcatenate(IEnumerable<string> textBlocks)
        {
            return string.Join("", textBlocks);
        }


        private static XElement TransformXmlTextToMetadata(string xmlText, IEnumerable<XElement> ccContent, bool parentisPara)
        {
            XElement xml;
            try
            {
                xml = XElement.Parse(xmlText);
                xml.Add(ccContent);
            }
            catch (XmlException e)
            {
                xml = parentisPara ?
                    XElementFactory.CreateRunErrorMessage("XmlException: " + e.Message) :
                    XElementFactory.CreateParaErrorMessage("XmlException: " + e.Message);

            }

            return xml;
        }

        
        //TODO копипаста с тестами

        private static List<T> MakeEmptyList<T>()
        {
            return new List<T>();
        }



    }



   

}
