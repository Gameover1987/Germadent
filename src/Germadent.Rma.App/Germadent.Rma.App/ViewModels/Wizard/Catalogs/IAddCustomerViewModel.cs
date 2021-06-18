using Germadent.Model;

namespace Germadent.Rma.App.ViewModels.Wizard.Catalogs
{
    public interface IAddCustomerViewModel
    {
        void Initialize(CardViewMode viewMode, CustomerDto customer);

        CustomerDto GetCustomer();
    }
}