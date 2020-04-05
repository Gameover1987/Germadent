using System.Collections.ObjectModel;
using Germadent.Rma.App.ServiceClient;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.Wizard.Catalogs
{
    public class CustomerCatalogViewModel : ViewModelBase, ICustomerCatalogViewModel
    {
        private readonly IRmaOperations _rmaOperations;

        private ICustomerViewModel _selectedCustomer;

        public CustomerCatalogViewModel(IRmaOperations rmaOperations)
        {
            _rmaOperations = rmaOperations;
        }

        public ObservableCollection<ICustomerViewModel> Customers { get; } = new ObservableCollection<ICustomerViewModel>();

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
            Customers.Clear();

            var customers = _rmaOperations.GetCustomers();
            foreach (var customer in customers)
            {
             Customers.Add(new CustomerViewModel());   
            }
        }
    }
}
