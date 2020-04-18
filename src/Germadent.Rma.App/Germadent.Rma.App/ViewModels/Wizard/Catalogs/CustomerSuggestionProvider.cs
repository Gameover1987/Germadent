using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.Rma.App.ServiceClient;
using Germadent.UI.Controls;

namespace Germadent.Rma.App.ViewModels.Wizard.Catalogs
{
    public class CustomerSuggestionProvider : ISuggestionProvider
    {
        private readonly IRmaOperations _rmaOperations;

        public CustomerSuggestionProvider(IRmaOperations rmaOperations)
        {
            _rmaOperations = rmaOperations;
        }

        public IEnumerable GetSuggestions(string filter)
        {
            return _rmaOperations.GetCustomers(filter).Select(x => new CustomerViewModel(x)).ToArray();
        }
    }
}
