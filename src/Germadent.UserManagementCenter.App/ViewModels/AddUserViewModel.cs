using System.Collections.ObjectModel;
using Germadent.UI.Commands;
using Germadent.UI.ViewModels;
using Germadent.UserManagementCenter.App.ServiceClient;
using Germadent.UserManagementCenter.Model;

namespace Germadent.UserManagementCenter.App.ViewModels
{
    public class AddUserViewModel : ViewModelBase, IAddUserViewModel
    {
        private readonly IUserManagementCenterOperations _userManagementCenterOperations;
        private string _fullName;
        private string _login;
        private string _password;
        private string _passwordOnceAgain;

        public AddUserViewModel(IUserManagementCenterOperations userManagementCenterOperations)
        {
            _userManagementCenterOperations = userManagementCenterOperations;
        }

        public string Title { get; private set; }

        public string FullName
        {
            get { return _fullName; }
            set
            {
                if (_fullName == value)
                    return;
                _fullName = value;
                OnPropertyChanged(() => FullName);
            }
        }

        public string Login
        {
            get { return _login; }
            set
            {
                if (_login == value)
                    return;
                _login = value;
                OnPropertyChanged(() => Login);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password == value)
                    return;
                _password = value;
                OnPropertyChanged(() => Password);
            }
        }

        public string PasswordOnceAgain
        {
            get { return _passwordOnceAgain; }
            set
            {
                if (_passwordOnceAgain == value)
                    return;
                _passwordOnceAgain = value;
                OnPropertyChanged(() => PasswordOnceAgain);
            }
        }

        public ObservableCollection<RoleViewModel> Roles { get; } = new ObservableCollection<RoleViewModel>();

        public IDelegateCommand OkCommand { get; }

        public void Initialize(UserDto userDto, string title)
        {
            Title = title;

            var roles = _userManagementCenterOperations.GetRoles();

            Roles.Clear();
            foreach (var role in roles)
            {
                Roles.Add(new RoleViewModel(role));
            }
        }

        public UserViewModel GetUser()
        {
            throw new System.NotImplementedException();
        }
    }
}