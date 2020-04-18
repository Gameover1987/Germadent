using Germadent.Rma.Model;

namespace Germadent.Rma.App.ViewModels.Wizard.Catalogs
{
    public interface IAddCustomerViewModel
    {
        void Initialize(string title, CustomerDto customer);

        CustomerDto GetCustomer();
    }
}