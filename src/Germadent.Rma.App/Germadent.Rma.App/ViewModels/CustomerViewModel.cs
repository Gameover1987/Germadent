using Germadent.Rma.Model;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels
{
    public interface ICustomerViewModel
    {
        int CustomerId { get; }

        string DisplayName { get; }

        string WebSite { get; }

        string Email { get;}

        string Phone { get; }

        string Description { get; }

        void Update(CustomerDto customer);

        CustomerDto ToDto();
    }

    public class CustomerViewModel : ViewModelBase, ICustomerViewModel
    {
        private CustomerDto _customer;

        public CustomerViewModel(CustomerDto customer)
        {
            _customer = customer;
        }

        public int CustomerId => _customer.Id;

        public string WebSite => _customer.WebSite;

        public string DisplayName => _customer.Name;

        public string Email => _customer.Email;

        public string Phone => _customer.Phone;

        public string Description => _customer.Description;

        public void Update(CustomerDto customer)
        {
            _customer = customer;
            OnPropertyChanged();
        }

        public CustomerDto ToDto()
        {
            return new CustomerDto
            {
                Id = CustomerId,
                Description = Description,
                Email = Email,
                Name = DisplayName,
                Phone = Phone,
                WebSite = WebSite
            };
        }
    }
}
