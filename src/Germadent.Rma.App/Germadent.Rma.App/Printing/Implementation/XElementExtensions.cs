using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Germadent.Rma.App.Printing.Implementation
{
    public static class XElementExtensions
    {

        public static IEnumerable<XElement> GetDescElementsByXName(this XElement xElement, XName descElName)
        {
            return xElement.DescendantsAndSelf(descElName);
        }


        

        public static IEnumerable<XElement> GetElementsWithNamesExcept(this IEnumerable<XElement> xElements, XName elName)
        {
            return xElements.Elements().Where(e => e.Name != elName);
        }


        public static IEnumerable<XElement> FilterElementsByAttrValue(this IEnumerable<XElement> xElements, XName attrName, string attrValue)
        {
            return xElements.Where(el => (string)el.Attribute(attrName) == attrValue);
        }

        public static IEnumerable<XElement> FilterElementsByAttrValues(this IEnumerable<XElement> xElements, XName attrName, string[] attrValues)
        {
            return xElements.Where(el => attrValues.Contains((string)el.Attribute(attrName)));
        }


        public static string GetElementAttribute(this XElement xElement, XName attrName)
        {
            return (string)xElement.Attribute(attrName);
        }
        
        public static string[] GetElementsAttribute(this IEnumerable<XElement> xElements, XName attrName)
        {
            return xElements.Select(el => el.GetElementAttribute(attrName)).ToArray();
        }
        

        


        public static void DropFromTree(this IEnumerable<XElement> xElements)
        {
            foreach (var el in xElements)
            {
                el.Remove();
            }

        }


        public static bool ParentHasXName(this XElement element, XName checkedName)
        {
            return element.Parent != null && element.Parent.Name == checkedName;
        }



        public static IEnumerable<XElement> GetBookmarks(this XElement el, XName bName, XName attrName, params string[] attrValues)
        {
            return el.GetDescElementsByXName(bName).FilterElementsByAttrValues(attrName, attrValues);
        }

    }
}
