using Germadent.Rma.Model;

namespace Germadent.Rma.App.ViewModels.Wizard.Catalogs
{
    public interface IAddCustomerViewModel
    {
        void Initialize(string title, CustomerDto customer);

        CustomerDto GetCustomer();
    }

    public interface IAddResponsiblePersonViewModel
    {
        void Initialize(string title, ResponsiblePersonDto responsiblePerson);

        CustomerDto GetResponsiblePerson();
    }

    public class AddResponsiblePersonViewModel : IAddResponsiblePersonViewModel
    {
        public void Initialize(string title, ResponsiblePersonDto responsiblePerson)
        {
            throw new System.NotImplementedException();
        }

        public CustomerDto GetResponsiblePerson()
        {
            throw new System.NotImplementedException();
        }
    }
}