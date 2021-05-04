using Germadent.Model;

namespace Germadent.Client.Common.ServiceClient
{
    public interface IDictionaryRepository : IRepository<DictionaryItemDto>
    {
        DictionaryItemDto[] GetItems(DictionaryType dictionary);
    }
}