using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Packaging;
using Newtonsoft.Json.Linq;

namespace Germadent.Rma.App.Reporting.TemplateProcessing
{
    public class WordJsonAssembler : IWordAssembler
    {
        
        private readonly XName[] _trackedRevisionsElements =
        {
          Wrd.CellDel,
          Wrd.CellIns,
          Wrd.CellMerge,
          Wrd.CustomXmlDelRangeEnd,
          Wrd.CustomXmlDelRangeStart,
          Wrd.CustomXmlInsRangeEnd,
          Wrd.CustomXmlInsRangeStart,
          Wrd.Del,
          Wrd.DelInstrText,
          Wrd.DelText,
          Wrd.Ins,
          Wrd.MoveFrom,
          Wrd.MoveFromRangeEnd,
          Wrd.MoveFromRangeStart,
          Wrd.MoveTo,
          Wrd.MoveToRangeEnd,
          Wrd.MoveToRangeStart,
          Wrd.NumberingChange,
          Wrd.PPrChange,
          Wrd.RPrChange,
          Wrd.SectPrChange,
          Wrd.TblGridChange,
          Wrd.TblPrChange,
          Wrd.TblPrExChange,
          Wrd.TcPrChange,
          Wrd.TrPrChange
        };

        
        public byte[] Assembly(byte[] templateDoc, string jsonString)
        {
            using (var mem = new MemoryStream())
            {
                mem.Write(templateDoc, 0, templateDoc.Length);
                using (var wordDoc = WordprocessingDocument.Open(mem, true))
                {
                    if (PartHasTrackedRevisions(wordDoc.MainDocumentPart))
                        throw new Exception("Invalid DocumentAssembler template - contains tracked revisions");


                    var jsonData = JObject.Parse(jsonString);
                    ProcessTemplatePart(wordDoc.MainDocumentPart, jsonData);
                }
                
                return mem.ToArray();
            }

        }

        
        private bool PartHasTrackedRevisions(OpenXmlPart part)
        {
            return GetXDocument(part).Descendants().Any(e => _trackedRevisionsElements.Contains(e.Name));
        }

        private static void ProcessTemplatePart(OpenXmlPart docPart, JToken jsonData)
        {
            var xDoc = GetXDocument(docPart);
            var xDocRoot = xDoc.Root;
            var xDocRootResult = WordXmlPartAssembler.Assemble(xDocRoot, jsonData);
            xDoc.Elements().First().ReplaceWith(xDocRootResult);
            PutXDocument(docPart);
        }

        private static XDocument GetXDocument(OpenXmlPart part)
        {
            try
            {
                var xdocument = part.Annotation<XDocument>();
                if (xdocument != null)
                    return xdocument;
                using (var stream = part.GetStream())
                {
                    if (stream.Length == 0L)
                    {
                        xdocument = new XDocument();
                        xdocument.Declaration = new XDeclaration("1.0", "UTF-8", "yes");
                    }
                    else
                    {
                        using (var reader = XmlReader.Create(stream))
                            xdocument = XDocument.Load(reader);
                    }
                }
                part.AddAnnotation(xdocument);
                return xdocument;
            }
            catch (Exception)
            {
                throw new Exception("Ошибка при преобразовании документа в XML");
            }
        }

        private static void PutXDocument(OpenXmlPart docPart)
        {
            XDocument xdocument = GetXDocument(docPart);
            if (xdocument == null)
                return;
            using (var stream = docPart.GetStream(FileMode.Create, FileAccess.Write))
            {
                using (var writer = XmlWriter.Create(stream))
                    xdocument.Save(writer);
            }
        }
    }
}