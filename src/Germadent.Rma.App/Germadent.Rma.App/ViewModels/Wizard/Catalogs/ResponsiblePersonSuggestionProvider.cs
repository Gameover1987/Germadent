using System.Collections;
using System.Linq;
using Germadent.Client.Common.ServiceClient;
using Germadent.Client.Common.ServiceClient.Repository;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.UI.Controls;

namespace Germadent.Rma.App.ViewModels.Wizard.Catalogs
{
    public interface IResponsiblePersonsSuggestionsProvider : ISuggestionProvider
    {

    }

    public class ResponsiblePersonSuggestionProvider : IResponsiblePersonsSuggestionsProvider
    {
        private readonly IResponsiblePersonRepository _responsiblePersonRepository;

        public ResponsiblePersonSuggestionProvider(IResponsiblePersonRepository responsiblePersonRepository)
        {
            _responsiblePersonRepository = responsiblePersonRepository;
        }

        public IEnumerable GetSuggestions(string filter)
        {
            return _responsiblePersonRepository.Items.Where(x => x.FullName.ToLower().Contains(filter.ToLower())).Select(x => new ResponsiblePersonViewModel(x)).ToArray();
        }
    }
}