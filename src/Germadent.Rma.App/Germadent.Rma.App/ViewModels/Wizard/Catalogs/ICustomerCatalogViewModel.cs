using Germadent.UI.Commands;

namespace Germadent.Rma.App.ViewModels.Wizard.Catalogs
{
    public interface ICustomerCatalogViewModel
    {
        string SearchString { get; set; }

        ICustomerViewModel SelectedCustomer { get; }

        IDelegateCommand AddCustomerCommand { get; }        

        void Initialize();
    }
}