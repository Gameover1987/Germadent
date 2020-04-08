namespace Germadent.Rma.App.ViewModels.Wizard.Catalogs
{
    public interface ICustomerCatalogViewModel
    {
        void Initialize();

        string SearchString { get; set; }

        ICustomerViewModel SelectedCustomer { get; }
    }
}