using System;
using System.Collections.ObjectModel;
using System.Linq;
using Germadent.Common.Extensions;
using Germadent.UI.Commands;
using Germadent.UI.ViewModels.Validation;
using Germadent.UserManagementCenter.App.ServiceClient;
using Germadent.UserManagementCenter.Model;

namespace Germadent.UserManagementCenter.App.ViewModels
{
    public enum ViewMode
    {
        Add, Edit
    }

    public class AddUserViewModel : ValidationSupportableViewModel, IAddUserViewModel
    {
        private readonly IUmcServiceClient _umcServiceClient;

        private int _userId;
        private string _firstName;
        private string _surName;
        private string _patronymic;
        private string _login;
        private string _password;
        private string _passwordOnceAgain;
        private string _description;
        private bool _isLocked;
        private string _phone;
        private bool _isLoading;
        private byte[] _image;

        public AddUserViewModel(IUmcServiceClient umcServiceClient)
        {
            _umcServiceClient = umcServiceClient;

            AddValidationFor(() => FirstName)
                .When(() => string.IsNullOrWhiteSpace(FirstName), () => "Укажите полное имя пользователя");
            AddValidationFor(() => Login)
                .When(() => string.IsNullOrWhiteSpace(Login), () => "Укажите логин");
            AddValidationFor(() => Password)
                .When(() => string.IsNullOrWhiteSpace(Password), () => "Укажите пароль");
            AddValidationFor(() => PasswordOnceAgain)
                .When(() => string.IsNullOrWhiteSpace(PasswordOnceAgain), () => "Повторите пароль")
                .When(() => Password != PasswordOnceAgain, () => "Пароли должны совпадать");

            OkCommand = new DelegateCommand(() => { }, CanOkCommandHandler);
            ChangeUserImageCommand = new DelegateCommand(ChangeUserImageCommandHandler);
        }

        public string Title { get; private set; }

        public ViewMode ViewMode { get; private set; }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (_isLoading = value)
                    return;
                _isLoading = value;
                OnPropertyChanged(() => IsLoading);
            }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (_firstName == value)
                    return;
                _firstName = value;
                OnPropertyChanged(() => FirstName);
            }
        }

        public string Surname
        {
            get { return _surName; }
            set
            {
                if (_surName == value)
                    return;
                _surName = value;
                OnPropertyChanged(() => Surname);
            }
        }

        public string Patronymic
        {
            get { return _patronymic; }
            set
            {
                if (_patronymic == value)
                    return;
                _patronymic = value;
                OnPropertyChanged(() => Patronymic);
            }
        }

        public string Phone
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

        public bool IsLocked
        {
            get { return _isLocked; }
            set
            {
                if (_isLocked == value)
                    return;
                _isLocked = value;
                OnPropertyChanged(() => IsLocked);
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

        public byte[] Image
        {
            get { return _image; }
            set
            {
                if (_image == value)
                    return;
                _image = value;
                OnPropertyChanged(() => Image);
            }
        }

        public ObservableCollection<RoleViewModel> Roles { get; } = new ObservableCollection<RoleViewModel>();

        public IDelegateCommand OkCommand { get; }

        public IDelegateCommand ChangeUserImageCommand { get; }

        public bool AtLeastOneRoleChecked
        {
            get { return Roles.Any(x => x.IsChecked); }
        }

        public void Initialize(UserDto user, ViewMode viewMode)
        {
            ViewMode = viewMode;
            if (ViewMode == ViewMode.Add)
            {
                Title = "Добавление пользователя";
            }
            else
            {
                Title = "Редактирование данных пользователя";
            }

            _userId = user.UserId;
            _firstName = user.FirstName;
            _surName = user.Surname;
            _patronymic = user.Patronymic;
            _phone = user.Phone;
            _password = user.Password;
            _passwordOnceAgain = user.Password;
            _login = user.Login;
            _isLocked = user.IsLocked;
            _description = user.Description;

            var roles = _umcServiceClient.GetRoles();

            foreach (var role in Roles)
            {
                role.Checked -= RoleOnChecked;
            }

            var selectedRoleIds = user.Roles?.Select(x => x.RoleId).ToArray();
            Roles.Clear();
            foreach (var role in roles)
            {
                var roleViewModel = new RoleViewModel(role);
                roleViewModel.Checked += RoleOnChecked;
                if (selectedRoleIds != null)
                    roleViewModel.IsChecked = selectedRoleIds.Contains(roleViewModel.RoleId);
                Roles.Add(roleViewModel);
            }

            OnPropertyChanged();
        }

        private void RoleOnChecked(object sender, EventArgs e)
        {
            OnPropertyChanged(() => AtLeastOneRoleChecked);
        }

        public UserDto GetUser()
        {
            return new UserDto
            {
                UserId = _userId,
                FirstName = FirstName,
                Surname = Surname,
                Patronymic = Patronymic,
                Phone = Phone,
                Login = Login,
                Password = Password,
                IsLocked = IsLocked,
                Description = Description,
                Roles = Roles.Where(x => x.IsChecked).Select(x => x.ToModel()).ToArray()
            };
        }

        private bool CanOkCommandHandler()
        {
            return IsValid();
        }

        private void ChangeUserImageCommandHandler()
        {

        }

        private async void SetUserImage(byte[] image)
        {
            _umcServiceClient.
        }

        private bool IsValid()
        {
            return !FirstName.IsNullOrWhiteSpace() &&
                   !Login.IsNullOrWhiteSpace() &&
                   !Password.IsNullOrWhiteSpace() &&
                   !PasswordOnceAgain.IsNullOrWhiteSpace() &&
                   Password == PasswordOnceAgain &&
                   AtLeastOneRoleChecked;
        }
    }
}