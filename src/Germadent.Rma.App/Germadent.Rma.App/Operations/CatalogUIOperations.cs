using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;
using Germadent.Rma.App.Views;
using Germadent.Rma.Model;
using Germadent.UI.Infrastructure;

namespace Germadent.Rma.App.Operations
{
    public class CatalogUIOperations : ICatalogUIOperations
    {
        private readonly IShowDialogAgent _dialogAgent;
        private readonly IAddCustomerViewModel _addCustomerViewModel;
        private readonly IAddResponsiblePersonViewModel _addResponsiblePersonViewModel;
        private readonly IRmaServiceClient _rmaServiceClient;
        
        public CatalogUIOperations(IShowDialogAgent dialogAgent,
            IAddCustomerViewModel addCustomerViewModel,
            IAddResponsiblePersonViewModel addResponsiblePersonViewModel, 
            IRmaServiceClient rmaServiceClient)
        {
            _dialogAgent = dialogAgent;
            _addCustomerViewModel = addCustomerViewModel;
            _addResponsiblePersonViewModel = addResponsiblePersonViewModel;
            _rmaServiceClient = rmaServiceClient;
        }

        public CustomerDto AddCustomer(CustomerDto customer)
        {
            _addCustomerViewModel.Initialize(CardViewMode.Add, customer);
            
            if (_dialogAgent.ShowDialog<AddCustomerWindow>(_addCustomerViewModel) != true)
                return null;

            var newCustomer = _addCustomerViewModel.GetCustomer();
            return _rmaServiceClient.AddCustomer(newCustomer);
        }

        public CustomerDto UpdateCustomer(CustomerDto customer)
        {
            _addCustomerViewModel.Initialize(CardViewMode.Edit, customer);

            if (_dialogAgent.ShowDialog<AddCustomerWindow>(_addCustomerViewModel) != true)
                return null;

            var updatedCustomer = _addCustomerViewModel.GetCustomer();
            return _rmaServiceClient.UpdateCustomer(updatedCustomer);
        }

        public ResponsiblePersonDto AddResponsiblePerson(ResponsiblePersonDto responsiblePersonDto)
        {
            _addResponsiblePersonViewModel.Initialize(CardViewMode.Add, responsiblePersonDto);

            if (_dialogAgent.ShowDialog<AddResponsiblePersonWindow>(_addResponsiblePersonViewModel) == false)
                return null;

            var newResponsiblePerson = _addResponsiblePersonViewModel.GetResponsiblePerson();
            return _rmaServiceClient.AddResponsiblePerson(newResponsiblePerson);
        }
    }
}