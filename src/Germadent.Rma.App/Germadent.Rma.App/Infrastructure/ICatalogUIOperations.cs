using Germadent.Rma.App.ViewModels;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Infrastructure
{
    public interface ICatalogUIOperations
    {
        void Initialize();

        ICustomerViewModel SelectCustomer(string mask);

        CustomerDto AddCustomer(CustomerDto customer);

        void ShowCustomerCart(CustomerDto customer);
    }
}