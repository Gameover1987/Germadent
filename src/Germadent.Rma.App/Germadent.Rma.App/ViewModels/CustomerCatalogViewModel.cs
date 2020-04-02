using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.Rma.App.ServiceClient;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels
{
    public class CustomerCatalogViewModel : ViewModelBase, ICustomerCatalogViewModel
    {
        private readonly IRmaOperations _rmaOperations;

        private ICustomerViewModel _selectedCustomer;

        public CustomerCatalogViewModel(IRmaOperations rmaOperations)
        {
            _rmaOperations = rmaOperations;
        }

        public ICustomerViewModel SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                if (_selectedCustomer == value)
                    return;
                _selectedCustomer = value;
                OnPropertyChanged(() => SelectedCustomer);
            }
        }

        public void Initialize()
        {
            
        }
    }
}
