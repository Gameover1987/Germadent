using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Germadent.Common.Extensions;
using Germadent.Common.FileSystem;
using Germadent.Model;
using Germadent.Model.Production;
using Germadent.UI.Commands;
using Germadent.UI.Infrastructure;
using Germadent.UI.ViewModels.Validation;
using Germadent.UserManagementCenter.App.ServiceClient;
using NLog;

namespace Germadent.UserManagementCenter.App.ViewModels
{
    public enum ViewMode
    {
        Add, Edit
    }

    public class AddUserViewModel : ValidationSupportableViewModel, IAddUserViewModel
    {
        private readonly IUmcServiceClient _umcServiceClient;
        private readonly IShowDialogAgent _dialogAgent;
        private readonly IFileManager _fileManager;
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
        private string _fileName;

        private bool _isAdmin;
        private bool _isModeller;
        private bool _isOperator;
        private bool _isTechnic;
        private int _modellerQualifyingRank;
        private int _technicQualifyingRank;

        public AddUserViewModel(IUmcServiceClient umcServiceClient, IShowDialogAgent dialogAgent, IFileManager fileManager)
        {
            _umcServiceClient = umcServiceClient;
            _dialogAgent = dialogAgent;
            _fileManager = fileManager;

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

        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (_fileName == value)
                    return;
                _fileName = value;
                OnPropertyChanged(() => FileName);
            }
        }

        public ObservableCollection<RoleViewModel> Roles { get; } = new ObservableCollection<RoleViewModel>();

        public bool IsAdmin
        {
            get { return _isAdmin; }
            set
            {
                if (_isAdmin == value)
                    return;
                _isAdmin = value;
                OnPropertyChanged(() => IsAdmin);
            }
        }

        public bool IsModeller
        {
            get { return _isModeller; }
            set
            {
                if (_isModeller == value)
                    return;
                _isModeller = value;
                OnPropertyChanged(() => IsModeller);
            }
        }

        public int ModellerQualifyingRank
        {
            get { return _modellerQualifyingRank; }
            set
            {
                if (_modellerQualifyingRank == value)
                    return;
                _modellerQualifyingRank = value;
                OnPropertyChanged(() => ModellerQualifyingRank);
            }
        }

        public bool IsTechnic
        {
            get { return _isTechnic; }
            set
            {
                if (_isTechnic == value)
                    return;
                _isTechnic = value;
                OnPropertyChanged(() => IsTechnic);
            }
        }

        public int TechnicQualifyingRank
        {
            get { return _technicQualifyingRank; }
            set
            {
                if (_technicQualifyingRank == value)
                    return;
                _technicQualifyingRank = value;
                OnPropertyChanged(() => TechnicQualifyingRank);
            }
        }

        public bool IsOperator
        {
            get { return _isOperator; }
            set
            {
                if (_isOperator == value)
                    return;
                _isOperator = value;
                OnPropertyChanged(() => IsOperator);
            }
        }

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
            _fileName = user.FileName;

            _image = null;
            if (_userId != 0)
                _image = _umcServiceClient.GetUserImage(_userId);

            var roles = _umcServiceClient.GetRoles().OrderBy(x => x.RoleName);
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

            _isModeller = false;
            var modellerInfo = user.Positions.FirstOrDefault(x => x.EmployeePosition == EmployeePosition.Modeller);
            if (modellerInfo != null)
            {
                _isModeller = true;
                _modellerQualifyingRank = modellerInfo.QualifyingRank;
            }

            _isTechnic = false;
            var technicInfo  = user.Positions.FirstOrDefault(x => x.EmployeePosition == EmployeePosition.Technic);
            if (technicInfo != null)
            {
                _isTechnic = true;
                _technicQualifyingRank = technicInfo.QualifyingRank;
            }

            _isAdmin = user.Positions.Any(x => x.EmployeePosition == EmployeePosition.Admin);
            _isOperator = user.Positions.Any(x => x.EmployeePosition == EmployeePosition.Operator);

            OnPropertyChanged();
        }

        private void RoleOnChecked(object sender, EventArgs e)
        {
            OnPropertyChanged(() => AtLeastOneRoleChecked);
        }

        public UserDto GetUser()
        {
            var positions = new List<EmployeePositionDto>();
            if (IsAdmin)
                positions.Add(new EmployeePositionDto{UserId = _userId, EmployeePosition = EmployeePosition.Admin, QualifyingRank = 1});

            if (IsModeller)
                positions.Add(new EmployeePositionDto { UserId = _userId, EmployeePosition = EmployeePosition.Modeller, QualifyingRank = ModellerQualifyingRank });

            if (IsTechnic)
                positions.Add(new EmployeePositionDto { UserId = _userId, EmployeePosition = EmployeePosition.Technic, QualifyingRank = TechnicQualifyingRank });

            if  (IsOperator)
                positions.Add(new EmployeePositionDto { UserId = _userId, EmployeePosition = EmployeePosition.Operator, QualifyingRank = 1 });

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
                Roles = Roles.Where(x => x.IsChecked).Select(x => x.ToModel()).ToArray(),
                Positions = positions.ToArray(),
                FileName = FileName
            };
        }

        private bool CanOkCommandHandler()
        {
            return IsValid();
        }

        private void ChangeUserImageCommandHandler()
        {
            var filter = "Изображения|*.jpg";
            if (_dialogAgent.ShowOpenFileDialog(filter, out var fileName) == false)
                return;

            FileName = fileName;
            Image = _fileManager.ReadAllBytes(FileName);
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