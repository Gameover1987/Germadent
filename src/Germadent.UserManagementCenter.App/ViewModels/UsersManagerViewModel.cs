using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Germadent.UI.Commands;
using Germadent.UI.ViewModels;
using Germadent.UserManagementCenter.App.ServiceClient;

namespace Germadent.UserManagementCenter.App.ViewModels
{
    public class UsersManagerViewModel : ViewModelBase, IUsersManagerViewModel
    {
        private readonly IUserManagementCenterOperations _userManagementCenterOperations;
        private UserViewModel _selectedUser;

        public UsersManagerViewModel(IUserManagementCenterOperations userManagementCenterOperations)
        {
            _userManagementCenterOperations = userManagementCenterOperations;

            AddUserCommand = new DelegateCommand(x => AddUserCommandHandler(), x => CanAddUserCommandHandler());
            EditUSerCommand = new DelegateCommand(x => EditUserCommandHandler(), x=> CanEditUserCommandHandler());
            DeleteUserCommand = new DelegateCommand(x => DeleteUserCommandHandler(), x => CanDeleteUserCommandHandler());
        }

        public ObservableCollection<UserViewModel> Users { get; } = new ObservableCollection<UserViewModel>();

        public UserViewModel SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                if (_selectedUser == value)
                    return;
                _selectedUser = value;
                OnPropertyChanged(() => SelectedUser);
            }
        }

        public IDelegateCommand AddUserCommand { get; }

        public IDelegateCommand EditUSerCommand { get; }

        public IDelegateCommand DeleteUserCommand { get;}

        public void Initialize()
        {
            var users = _userManagementCenterOperations.GetUsers();
            Users.Clear();
            foreach (var user in users)
            {
                Users.Add(new UserViewModel(user));
            }
        }

        private bool CanAddUserCommandHandler()
        {
            return true;
        }

        private void AddUserCommandHandler()
        {

        }

        private bool CanEditUserCommandHandler()
        {
            return SelectedUser != null;
        }

        private void EditUserCommandHandler()
        {

        }

        private bool CanDeleteUserCommandHandler()
        {
            return SelectedUser != null;
        }

        private void DeleteUserCommandHandler()
        {

        }
    }
}
