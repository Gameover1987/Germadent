using Germadent.Model;

namespace Germadent.WebApi.Entities
{
    public class DictionaryItemEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string DictionaryName { get; set; }

        public DictionaryType DictionaryType { get; set; }
    }
}
