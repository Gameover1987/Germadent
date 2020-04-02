namespace Germadent.Rma.App.ViewModels
{
    public interface ICustomerCatalogViewModel
    {
        void Initialize();

        ICustomerViewModel SelectedCustomer { get; }
    }
}