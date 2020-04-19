using Germadent.Rma.App.ViewModels;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;
using Germadent.Rma.App.Views;
using Germadent.Rma.Model;
using Germadent.UI.Infrastructure;
using Microsoft.Practices.Unity;

namespace Germadent.Rma.App.Infrastructure
{
    public class CatalogUIOperations : ICatalogUIOperations
    {
        private readonly IShowDialogAgent _dialogAgent;
        private ICustomerCatalogViewModel _customerCatalogViewModel;
        private readonly IAddCustomerViewModel _addCustomerViewModel;
        private readonly IUnityContainer _unityContainer;

        public CatalogUIOperations(IShowDialogAgent dialogAgent, IAddCustomerViewModel addCustomerViewModel, IUnityContainer unityContainer)
        {
            _dialogAgent = dialogAgent;
            _addCustomerViewModel = addCustomerViewModel;
            _unityContainer = unityContainer;
        }

        public void Initialize()
        {
            _customerCatalogViewModel = _unityContainer.Resolve<ICustomerCatalogViewModel>();
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