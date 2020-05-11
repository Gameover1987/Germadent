using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ViewModels;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;
using Germadent.Rma.App.Views;
using Germadent.Rma.Model;
using Germadent.UI.Infrastructure;
using Unity;

namespace Germadent.Rma.App.Infrastructure
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
            _addCustomerViewModel.Initialize("Добавление нового заказчика", customer);
            
            if (_dialogAgent.ShowDialog<AddCustomerWindow>(_addCustomerViewModel) != true)
                return null;

            var newCustomer = _addCustomerViewModel.GetCustomer();
            return _rmaServiceClient.AddCustomer(newCustomer);
        }

        public ResponsiblePersonDto AddResponsiblePersons(ResponsiblePersonDto customerDto)
        {
            throw new System.NotImplementedException();
        }
    }
}