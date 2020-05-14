using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.ServiceClient.Repository
{
    public interface IDictionaryRepository : IRepository<DictionaryItemDto>
    {
        DictionaryItemDto[] GetItems(DictionaryType dictionary);
    }

    public class DictionaryRepository : Repository<DictionaryItemDto>, IDictionaryRepository
    {
        private readonly IRmaServiceClient _rmaServiceClient;

        public DictionaryRepository(IRmaServiceClient rmaServiceClient)
        {
            _rmaServiceClient = rmaServiceClient;
        }

        protected override DictionaryItemDto[] GetItems()
        {
            var allItems = new List<DictionaryItemDto>();
            var dictionaries = Enum.GetValues(typeof(DictionaryType)).Cast<DictionaryType>().ToArray();
            foreach (var dictionaryType in dictionaries)
            {
                var itemsFromDictionary = _rmaServiceClient.GetDictionary(dictionaryType);
                allItems.AddRange(itemsFromDictionary);
            }

            return allItems.ToArray();
        }


        public DictionaryItemDto[] GetItems(DictionaryType dictionary)
        {
            return GetItems().Where(x => x.Dictionary == dictionary).OrderBy(x => x.Name).ToArray();
        }
    }
}
