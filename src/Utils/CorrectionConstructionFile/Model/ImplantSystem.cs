using Germadent.Common.Extensions;

namespace Germadent.CorrectionConstructionFile.App.Model
{
    public class ImplantSystem
    {
        public string Name { get; set; }

        public CorrectionDictionaryItem[] CorrectionDictionary { get; set; }
    }

    public class CorrectionDictionaryItem
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public bool IsEmpty
        {
            get { return Name.IsNullOrWhiteSpace() && Value.IsNullOrWhiteSpace();}
        }
    }
}