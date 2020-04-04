namespace Germadent.Rma.App.ViewModels.Wizard.Catalogs
{
    public interface ICustomerCatalogViewModel
    {
        void Initialize();

        ICustomerViewModel SelectedCustomer { get; }
    }
}