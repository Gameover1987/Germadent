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
        private ICustomerCatalogViewModel _customerCatalogViewModel;
        private readonly IAddCustomerViewModel _addCustomerViewModel;
        private readonly IUnityContainer _unityContainer;
        private readonly IRmaServiceClient _rmaOperations;

        //TODO Nekrasov: где проверки на null?
        public CatalogUIOperations(IShowDialogAgent dialogAgent, IAddCustomerViewModel addCustomerViewModel, IUnityContainer unityContainer, IRmaServiceClient rmaOperations)
        {
            _dialogAgent = dialogAgent;
            _addCustomerViewModel = addCustomerViewModel;
            _unityContainer = unityContainer;
            _rmaOperations = rmaOperations;
        }

        public void Initialize()
        {
            //TODO Nekrasov:плохая практика пихать сюда DI контейнер, ничего страшного, но лучше избегать если есть возможность
            _customerCatalogViewModel = _unityContainer.Resolve<ICustomerCatalogViewModel>();
        }

        public ICustomerViewModel SelectCustomer(string mask)
        {
            _customerCatalogViewModel.SearchString = mask;
            //TODO Nekrasov: инвертировать
            if (_dialogAgent.ShowDialog<CustomerCatalogWindow>(_customerCatalogViewModel) == true)
            {
                return _customerCatalogViewModel.SelectedCustomer;
            }

            return null;
        }

        public CustomerDto AddCustomer(CustomerDto customer)
        {
            _addCustomerViewModel.Initialize("Добавление нового заказчика", customer);
            //TODO Nekrasov:инвертировать
            if (_dialogAgent.ShowDialog<AddCustomerWindow>(_addCustomerViewModel) != true)
                return null;

            var newCustomer = _addCustomerViewModel.GetCustomer();
            return _rmaOperations.AddCustomer(newCustomer);

        }

        public void ShowCustomerCart(CustomerDto customer)
        {
            _addCustomerViewModel.Initialize("Просмотр карточки заказчика", customer);
            _dialogAgent.ShowDialog<AddCustomerWindow>(_addCustomerViewModel);
        }

        public ResponsiblePersonDto AddResponsiblePersons(ResponsiblePersonDto customerDto)
        {
            throw new System.NotImplementedException();
        }
    }
}