using Germadent.Rma.App.ViewModels;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;
using Germadent.Rma.App.Views;
using Germadent.Rma.Model;
using Germadent.UI.Infrastructure;

namespace Germadent.Rma.App.Infrastructure
{
    public class CatalogUIOperations : ICatalogUIOperations
    {
        private readonly IShowDialogAgent _dialogAgent;
        private readonly ICustomerCatalogViewModel _customerCatalogViewModel;
        private readonly IAddCustomerViewModel _addCustomerViewModel;

        public CatalogUIOperations(IShowDialogAgent dialogAgent, ICustomerCatalogViewModel customerCatalogViewModel, IAddCustomerViewModel addCustomerViewModel)
        {
            _dialogAgent = dialogAgent;
            _customerCatalogViewModel = customerCatalogViewModel;
            _addCustomerViewModel = addCustomerViewModel;
        }

        public ICustomerViewModel SelectCustomer(string mask)
        {
            _customerCatalogViewModel.SearchString = mask;
            if (_dialogAgent.ShowDialog<CustomerCatalogWindow>(_customerCatalogViewModel) == true)
            {
                return _customerCatalogViewModel.SelectedCustomer;
            }

            return null;
        }

        public CustomerDto AddCustomer()
        {
            _addCustomerViewModel.Initialize("Добавление нового заказчика", new CustomerDto());
            if (_dialogAgent.ShowDialog<AddCustomerWindow>(_addCustomerViewModel) == true)
            {
                return _addCustomerViewModel.GetCustomer();
            }
            return null;
        }
    }
}