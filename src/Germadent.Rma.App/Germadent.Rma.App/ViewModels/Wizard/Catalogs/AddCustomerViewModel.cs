using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.Wizard.Catalogs
{
    public class AddCustomerViewModel : ViewModelBase
    {
        private string _name;
        private string _phone;
        private string _email;
        private string _webSite;
        private string _description;

        public AddCustomerViewModel()
        {
            
        }

        private string Name
        {
            get { return _name; }
            set
            {
                if (_name == value)
                    return;
                _name = value;
                OnPropertyChanged(() => Name);
            }
        }

        private string Phone
        {
            get { return _phone; }
            set
            {
                if (_phone == value)
                    return;
                _phone = value;
                OnPropertyChanged(() => Phone);
            }
        }


        private string Email
        {
            get { return _email; }
            set
            {
                if (_email == value)
                    return;
                _email = value;
                OnPropertyChanged(() => Email);
            }
        }

        private string WebSite
        {
            get { return _webSite; }
            set
            {
                if (_webSite == value)
                    return;
                _webSite = value;
                OnPropertyChanged(() => Name);
            }
        }
    }
}
