using Germadent.Rma.Model;
using Germadent.UI.Commands;
using Germadent.UI.ViewModels.Validation;

namespace Germadent.Rma.App.ViewModels.Wizard.Catalogs
{
    public class AddCustomerViewModel : ValidationSupportableViewModel, IAddCustomerViewModel
    {
        private int _id;
        private string _name;
        private string _phone;
        private string _email;
        private string _webSite;
        private string _description;

        public AddCustomerViewModel()
        {
            AddValidationFor(() => Name)
                .When(() => string.IsNullOrWhiteSpace(Name), () => "Укажите наименование заказчика");

            OkCommand = new DelegateCommand(() => { }, CanOkCommandHandler);
        }

        public string Title { get; private set; }

        public string Name
        {
            get => _name;
            set
            {
                if (_name == value)
                    return;
                _name = value;
                OnPropertyChanged(() => Name);
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                if (_phone == value)
                    return;
                _phone = value;
                OnPropertyChanged(() => Phone);
            }
        }


        public string Email
        {
            get => _email;
            set
            {
                if (_email == value)
                    return;
                _email = value;
                OnPropertyChanged(() => Email);
            }
        }

        public string WebSite
        {
            get => _webSite;
            set
            {
                if (_webSite == value)
                    return;
                _webSite = value;
                OnPropertyChanged(() => Name);
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (_description == value)
                    return;
                _description = value;
                OnPropertyChanged(() => Description);
            }
        }

        public IDelegateCommand OkCommand { get; }

        public void Initialize(string title, CustomerDto customer)
        {
            Title = title;

            _id = customer.Id;
            Name = customer.Name;
            Phone = customer.Phone;
            Email = customer.Email;
            WebSite = customer.WebSite;
            Description = customer.Description;
        }

        public CustomerDto GetCustomer()
        {
            return new CustomerDto
            {
                Id = _id,
                Name = Name,
                Phone = Phone,
                WebSite = WebSite,
                Email = Email,
                Description = Description
            };
        }

        private bool CanOkCommandHandler()
        {
            return !HasErrors && !string.IsNullOrWhiteSpace(Name);
        }
    }
}
