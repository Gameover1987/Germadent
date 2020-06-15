using System;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace Germadent.Rma.App.Reporting.TemplateProcessing
{
    public static class XElementFactory
    {
        public static XElement Clone(XElement el)
        {
            return new XElement(el);
        }



        public static XElement MakeEmptyPara()
        {
            return new XElement(Wrd.P, new XElement(Wrd.R));
        }

        public static XElement MakeEmptyRun()
        {
            return new XElement(Wrd.R);
        }


        public static XElement Make(XName elName, params object[] attrsAndChildren)
        {
            return new XElement(elName, attrsAndChildren);
        }

        public static XElement MakeRecursively(XElement el, Func<XNode, object> recFunc)
        {
            return new XElement(el.Name,
                   el.Attributes(),
                   el.Nodes().Select(recFunc));
        }


        public static XElement MakeRecursively(XElement el, JToken json,  Func<XNode, JToken, object> recFunc)
        {
            return new XElement(el.Name,
                   el.Attributes(),
                   el.Nodes().Select(n => recFunc(n, json)));
        }

        public static XAttribute MakeAttribute(XName name, string value)
        {
            return new XAttribute(name, value);
        }


        public static XElement CreateContextErrorMessage(XElement element, string errorMessage)
        {
            var para = element.Descendants(Wrd.P).FirstOrDefault();
            
            if (para != null)
                return CreateParaErrorMessage(errorMessage);

            return CreateRunErrorMessage(errorMessage);
        }


        public static XElement CreateRunErrorMessage(string errorMessage)
        {
            return Make(Wrd.R,
                            Make(Wrd.RPr,
                                Make(Wrd.Color, MakeAttribute(Wrd.Val, "FF0000")),
                                Make(Wrd.Highlight, MakeAttribute(Wrd.Val, "yellow"))),
                            Make(Wrd.T, errorMessage));
            
        }

        public static XElement CreateParaErrorMessage(string errorMessage)
        {
            return Make(Wrd.P, CreateRunErrorMessage(errorMessage));
        }


        public static XElement CreateInvalidMetaTagMarkdownErrorMessage(string otherText)
        {
            return Make(Wrd.P,
                            CreateRunErrorMessage("Error: Meta tag cannot be in paragraph with other text"),
                            Make(Wrd.R, Make(Wrd.T, otherText)));
        }


    }
}
