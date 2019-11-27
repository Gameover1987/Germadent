using System.Xml.Linq;

namespace Germadent.Rma.App.Printing.Implementation
{
    public static class MetaNames
    {
        public static XName Content = "Content";
        public static XName Counter = "Counter";

        public static XName Table = "Table";
        public static XName Repeat = "Repeat";
        public static XName EndRepeat = "EndRepeat";
        public static XName Conditional = "Conditional";
        public static XName EndConditional = "EndConditional";

        public static XName DefaultValue = "DefaultValue";
        public static XName Select = "Select";
        public static XName Match = "Match";
        public static XName NotMatch = "NotMatch";
        public static XName Depth = "Depth";
        public static XName DateFormat = "DateFormat";
    }
}