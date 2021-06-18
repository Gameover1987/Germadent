using Germadent.Model;

namespace Germadent.Client.Common.ServiceClient.Repository
{
    public interface IDictionaryRepository : IRepository<DictionaryItemDto>
    {
        DictionaryItemDto[] GetItems(DictionaryType dictionary);
    }
}