using System;
using Germadent.UI.Infrastructure;
using Germadent.UserManagementCenter.App.ViewModels;
using Germadent.UserManagementCenter.Model;

namespace Germadent.UserManagementCenter.App.Views
{
    public sealed class WindowManager : IWindowManager
    {
        private readonly IShowDialogAgent _dialogAgent;
        private readonly IAddUserViewModel _addUserViewModel;
        private readonly IAddRoleViewModel _addRoleViewModel;

        public WindowManager(IShowDialogAgent dialogAgent, IAddUserViewModel addUserViewModel, IAddRoleViewModel addRoleViewModel)
        {
            _dialogAgent = dialogAgent;
            _addUserViewModel = addUserViewModel;
            _addRoleViewModel = addRoleViewModel;
        }

        public UserViewModel AddUser()
        {
            _addUserViewModel.Initialize(new UserDto(), "Добавление нового пользователя");
            if (_dialogAgent.ShowDialog<AddUserWindow>(_addUserViewModel) == true)
            {
                return new UserViewModel(_addUserViewModel.GetUser());
            }

            return null;
        }

        public UserViewModel EditUser(UserViewModel user)
        {
            throw new NotImplementedException();
        }

        public RoleViewModel AddRole()
        {
            _addRoleViewModel.Initialize(new RoleDto(), "Добавление новой роли");
            if (_dialogAgent.ShowDialog<AddRoleWindow>(_addRoleViewModel) == true)
            {
                return new RoleViewModel(_addRoleViewModel.GetRole());
            }

            return null;
        }
    }
}