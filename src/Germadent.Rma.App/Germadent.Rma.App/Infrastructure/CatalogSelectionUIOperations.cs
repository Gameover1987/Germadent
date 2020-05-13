using Germadent.Rma.App.ViewModels.Wizard.Catalogs;
using Germadent.Rma.App.Views;
using Germadent.Rma.Model;
using Germadent.UI.Infrastructure;

namespace Germadent.Rma.App.Infrastructure
{
    public class CatalogSelectionUIOperations : ICatalogSelectionUIOperations
    {
        private readonly IShowDialogAgent _dialogAgent;
        private readonly ICustomerCatalogViewModel _customerCatalogViewModel;
        private readonly IAddCustomerViewModel _addCustomerViewModel;
        private readonly IResponsiblePersonCatalogViewModel _responsiblePersonCatalogViewModel;
        private readonly IAddResponsiblePersonViewModel _addResponsiblePersonViewModel;

        public CatalogSelectionUIOperations(IShowDialogAgent dialogAgent,
            ICustomerCatalogViewModel customerCatalogViewModel,
            IAddCustomerViewModel addCustomerViewModel,
            IResponsiblePersonCatalogViewModel responsiblePersonCatalogViewModel,
            IAddResponsiblePersonViewModel addResponsiblePersonViewModel)
        {
            _dialogAgent = dialogAgent;
            _customerCatalogViewModel = customerCatalogViewModel;
            _addCustomerViewModel = addCustomerViewModel;
            _responsiblePersonCatalogViewModel = responsiblePersonCatalogViewModel;
            _addResponsiblePersonViewModel = addResponsiblePersonViewModel;
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
            _addCustomerViewModel.Initialize(CardViewMode.View, customer);
            _dialogAgent.ShowDialog<AddCustomerWindow>(_addCustomerViewModel);
        }

        public ResponsiblePersonDto SelectResponsiblePerson(string mask)
        {
            _responsiblePersonCatalogViewModel.SearchString = mask;

            if (_dialogAgent.ShowDialog<ResponsiblePersonCatalogWindow>(_responsiblePersonCatalogViewModel) != true)
                return null;

            return _responsiblePersonCatalogViewModel.SelectedResponsiblePerson.ToDto();
        }

        public void ShowResponsiblePersonCard(ResponsiblePersonDto responsiblePerson)
        {
            _addResponsiblePersonViewModel.Initialize(CardViewMode.View, responsiblePerson);
            _dialogAgent.ShowDialog<AddResponsiblePersonWindow>(_addResponsiblePersonViewModel);
        }
    }
}