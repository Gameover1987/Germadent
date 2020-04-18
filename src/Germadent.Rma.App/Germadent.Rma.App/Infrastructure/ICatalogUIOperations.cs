using Germadent.Rma.App.ViewModels;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Infrastructure
{
    public interface ICatalogUIOperations
    {
        ICustomerViewModel SelectCustomer(string mask);

        CustomerDto AddCustomer();
    }
}