using System;
using System.Collections.ObjectModel;
using System.Linq;
using Germadent.UI.Commands;
using Germadent.UI.ViewModels;
using Germadent.UI.ViewModels.Validation;
using Germadent.UserManagementCenter.App.ServiceClient;
using Germadent.UserManagementCenter.Model;

namespace Germadent.UserManagementCenter.App.ViewModels
{
    public class AddUserViewModel : ValidationSupportableViewModel, IAddUserViewModel
    {
        private readonly IUmcOperations _userManagementCenterOperations;
        private string _fullName;
        private string _login;
        private string _password;
        private string _passwordOnceAgain;
        private string _description;

        public AddUserViewModel(IUmcOperations userManagementCenterOperations)
        {
            _userManagementCenterOperations = userManagementCenterOperations;

            AddValidationFor(() => FullName)
                .When(() => string.IsNullOrWhiteSpace(FullName), () => "Укажите полное имя пользователя");
            AddValidationFor(() => Login)
                .When(() => string.IsNullOrWhiteSpace(Login), () => "Укажите логин");
            AddValidationFor(() => Password)
                .When(() => string.IsNullOrWhiteSpace(Password), () => "Укажите пароль");
            AddValidationFor(() => PasswordOnceAgain)
                .When(() => string.IsNullOrWhiteSpace(PasswordOnceAgain), () => "Повторите пароль")
                .When(() => Password != PasswordOnceAgain, () => "Пароли должны совпадать");

            OkCommand = new DelegateCommand(() => { }, CanOkCommandHandler);
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

        public string Description
        {
            get { return _description; }
            set
            {
                if (_description == value)
                    return;
                _description = value;
                OnPropertyChanged(() => Description);
            }
        }

        public ObservableCollection<RoleViewModel> Roles { get; } = new ObservableCollection<RoleViewModel>();

        public IDelegateCommand OkCommand { get; }

        public bool AtLeastOneRoleChecked
        {
            get { return Roles.Any(x => x.IsChecked); }
        }

        public void Initialize(UserDto user, string title)
        {
            Title = title;

            var roles = _userManagementCenterOperations.GetRoles();

            foreach (var role in Roles)
            {
                role.Checked -= RoleOnChecked;
            }

            Roles.Clear();
            foreach (var role in roles)
            {
                var roleViewModel = new RoleViewModel(role);
                roleViewModel.Checked += RoleOnChecked;
                Roles.Add(roleViewModel);
            }
        }

        private void RoleOnChecked(object sender, EventArgs e)
        {
            OnPropertyChanged(() => AtLeastOneRoleChecked);
        }

        public UserDto GetUser()
        {
            return new UserDto
            {
                FullName = FullName,
                Login = Login,
                Password = Password,
                Roles = Roles.Where(x => x.IsChecked).Select(x => x.ToModel()).ToArray()
            };
        }

        private bool CanOkCommandHandler()
        {
            return AtLeastOneRoleChecked && !HasErrors;
        }
    }
}