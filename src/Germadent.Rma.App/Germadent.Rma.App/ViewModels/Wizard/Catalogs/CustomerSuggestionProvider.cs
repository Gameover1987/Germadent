using System.Collections;
using System.Linq;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.UI.Controls;
using Germadent.UI.Infrastructure;

namespace Germadent.Rma.App.ViewModels.Wizard.Catalogs
{
    public class CustomerSuggestionProvider : SuggestionsProviderBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerSuggestionProvider(ICustomerRepository customerRepository, IDispatcher dispatcher)
            : base(dispatcher)
        {
            _customerRepository = customerRepository;
        }

        protected override IEnumerable GetSuggestionsImpl(string filter)
        {
            return _customerRepository.Items.Where(x => x.Name.ToLower().Contains(filter.ToLower())).Select(x => new CustomerViewModel(x)).ToArray();
        }
    }
}
