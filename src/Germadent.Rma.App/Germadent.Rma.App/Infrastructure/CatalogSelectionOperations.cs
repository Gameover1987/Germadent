using Germadent.Rma.App.ViewModels.Wizard.Catalogs;
using Germadent.Rma.App.Views;
using Germadent.Rma.Model;
using Germadent.UI.Infrastructure;

namespace Germadent.Rma.App.Infrastructure
{
    public class CatalogSelectionOperations : ICatalogSelectionOperations
    {
        private readonly IShowDialogAgent _dialogAgent;
        private readonly ICustomerCatalogViewModel _customerCatalogViewModel;
        private readonly IAddCustomerViewModel _addCustomerViewModel;
        private readonly IResponsiblePersonCatalogViewModel _responsiblePersonCatalogViewModel;

        public CatalogSelectionOperations(IShowDialogAgent dialogAgent,
            ICustomerCatalogViewModel customerCatalogViewModel,
            IAddCustomerViewModel addCustomerViewModel, 
            IResponsiblePersonCatalogViewModel responsiblePersonCatalogViewModel)
        {
            _dialogAgent = dialogAgent;
            _customerCatalogViewModel = customerCatalogViewModel;
            _addCustomerViewModel = addCustomerViewModel;
            _responsiblePersonCatalogViewModel = responsiblePersonCatalogViewModel;
        }

        public CustomerDto SelectCustomer(string mask)
        {
            _customerCatalogViewModel.SearchString = mask;

            if (_dialogAgent.ShowDialog<CustomerCatalogWindow>(_customerCatalogViewModel) != true)
                return null;

            return _customerCatalogViewModel.SelectedCustomer.ToDto();
        }

        public void ShowCustomerCart(CustomerDto customer)
        {
            _addCustomerViewModel.Initialize("Просмотр карточки заказчика", customer);
            _dialogAgent.ShowDialog<AddCustomerWindow>(_addCustomerViewModel);
        }

        public ResponsiblePersonDto SelectResponsiblePerson(string mask)
        {
            _responsiblePersonCatalogViewModel.SearchString = mask;

            if (_dialogAgent.ShowDialog<ResponsiblePersonCatalogWindow>(_responsiblePersonCatalogViewModel) != true)
                return null;

            return _responsiblePersonCatalogViewModel.SelectedResponsiblePerson.ToDto();
        }
    }
}