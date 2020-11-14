using System.Windows;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;
using Germadent.Rma.App.Views;
using Germadent.Rma.Model;
using Germadent.Rma.Model.Pricing;
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

        public DeleteResult DeleteCustomer(int customerId)
        {
            var questionMsg = "Действительно хотите удалить заказчика?";
            if (_dialogAgent.ShowMessageDialog(questionMsg, Rma.App.Properties.Resources.AppTitle, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return null;

            var result =  _rmaServiceClient.DeleteCustomer(customerId);
            if (result.Count == 0)
            {
                var resultMsg = "Нельзя удалить заказчика на которого назначены заказ-наряды!";
                _dialogAgent.ShowMessageDialog(resultMsg, Rma.App.Properties.Resources.AppTitle);
                return null;
            }

            return result;
        }

        public ResponsiblePersonDto AddResponsiblePerson(ResponsiblePersonDto responsiblePersonDto)
        {
            _addResponsiblePersonViewModel.Initialize(CardViewMode.Add, responsiblePersonDto);

            if (_dialogAgent.ShowDialog<AddResponsiblePersonWindow>(_addResponsiblePersonViewModel) == false)
                return null;

            var newResponsiblePerson = _addResponsiblePersonViewModel.GetResponsiblePerson();
            return _rmaServiceClient.AddResponsiblePerson(newResponsiblePerson);
        }

        public ResponsiblePersonDto UpdateResponsiblePerson(ResponsiblePersonDto responsiblePersonDto)
        {
            _addResponsiblePersonViewModel.Initialize(CardViewMode.Edit, responsiblePersonDto);

            if (_dialogAgent.ShowDialog<AddResponsiblePersonWindow>(_addResponsiblePersonViewModel) == false)
                return null;

            var updatedResponsblePerson = _addResponsiblePersonViewModel.GetResponsiblePerson();
            return _rmaServiceClient.UpdateResponsiblePerson(updatedResponsblePerson);
        }

        public DeleteResult DeleteResponsiblePerson(int responsiblePersonId)
        {
            var questionMsg = "Действительно хотите удалить ответственное лицо?";
            if (_dialogAgent.ShowMessageDialog(questionMsg, Rma.App.Properties.Resources.AppTitle, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return null;

            var result = _rmaServiceClient.DeleteResponsiblePerson(responsiblePersonId);
            if (result.Count == 0)
            {
                var resultMsg = "Нельзя удалить доктора или техника указанного в заказ-нарядах!";
                _dialogAgent.ShowMessageDialog(resultMsg, Rma.App.Properties.Resources.AppTitle);
                return null;
            }

            return result;
        }
    }
}