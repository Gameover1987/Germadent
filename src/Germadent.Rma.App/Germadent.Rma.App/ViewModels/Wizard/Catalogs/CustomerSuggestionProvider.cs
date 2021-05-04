using System.Collections;
using System.Linq;
using Germadent.Client.Common.ServiceClient;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.UI.Controls;

namespace Germadent.Rma.App.ViewModels.Wizard.Catalogs
{
    public interface ICustomerSuggestionProvider : ISuggestionProvider
    {
    }

    public class CustomerSuggestionProvider : ICustomerSuggestionProvider
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerSuggestionProvider(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public IEnumerable GetSuggestions(string filter)
        {
            return _customerRepository.Items.Where(x => x.Name.ToLower().Contains(filter.ToLower())).Select(x => new CustomerViewModel(x)).ToArray();
        }
    }
}
