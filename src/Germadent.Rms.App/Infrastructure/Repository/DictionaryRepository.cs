using System;
using System.Collections.Generic;
using System.Linq;
using Germadent.Client.Common.ServiceClient;
using Germadent.Model;
using Germadent.Rms.App.ServiceClient;

namespace Germadent.Rms.App.Infrastructure.Repository
{
    public class DictionaryRepository : Repository<DictionaryItemDto>, IDictionaryRepository
    {
        private readonly IRmsServiceClient _rmsServiceClient;

        public DictionaryRepository(IRmsServiceClient rmsServiceClient)
        {
            _rmsServiceClient = rmsServiceClient;
        }

        protected override DictionaryItemDto[] GetItems()
        {
            var allItems = new List<DictionaryItemDto>();
            var dictionaries = Enum.GetValues(typeof(DictionaryType)).Cast<DictionaryType>().ToArray();
            foreach (var dictionaryType in dictionaries)
            {
                var itemsFromDictionary = _rmsServiceClient.GetDictionary(dictionaryType);
                allItems.AddRange(itemsFromDictionary);
            }

            return allItems.ToArray();
        }

        public DictionaryItemDto[] GetItems(DictionaryType dictionary)
        {
            return Items.Where(x => x.Dictionary == dictionary).OrderBy(x => x.Name).ToArray();
        }
    }
}
