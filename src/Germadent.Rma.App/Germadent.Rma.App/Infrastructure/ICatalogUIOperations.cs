using Germadent.Rma.App.ViewModels;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Infrastructure
{
    public interface ICatalogUIOperations
    {
        CustomerDto AddCustomer(CustomerDto customer);

        ResponsiblePersonDto AddResponsiblePersons(ResponsiblePersonDto customerDto);
    }

    public interface ICatalogSelectionOperations
    {
        CustomerDto SelectCustomer(string mask);

        void ShowCustomerCart(CustomerDto customer);

        ResponsiblePersonDto SelectResponsiblePerson(string mask);

    }
}