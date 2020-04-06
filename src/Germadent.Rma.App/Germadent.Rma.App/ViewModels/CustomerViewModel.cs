using Germadent.Rma.Model;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels
{
    public interface ICustomerViewModel
    {
        int CustomerId { get; }

        string DisplayName { get; }

        string WebSite { get; set; }

        string Email { get;}

        string Phone { get; }

        string Description { get; }
    }

    public class CustomerViewModel : ViewModelBase, ICustomerViewModel
    {
        public CustomerViewModel(CustomerDto customer)
        {
            CustomerId = customer.Id;
            DisplayName = customer.Name;
            Phone = customer.Phone;
            Description = customer.Description;
            Email = customer.Email;
            WebSite = customer.WebSite;
        }

        public int CustomerId { get; }

        public string WebSite { get; set; }

        public string DisplayName { get; }

        public string Email { get; set; }

        public string Phone { get; }

        public string Description { get; }
    }
}