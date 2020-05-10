using System;
using Germadent.UI.Infrastructure;
using Germadent.UserManagementCenter.App.ViewModels;
using Germadent.UserManagementCenter.App.Views;
using Germadent.UserManagementCenter.Model;

namespace Germadent.UserManagementCenter.App.UIOperations
{
    public sealed class UserManagementUIOperations : IUserManagementUIOperations
    {
        private readonly IShowDialogAgent _dialogAgent;
        private readonly IAddUserViewModel _addUserViewModel;
        private readonly IAddRoleViewModel _addRoleViewModel;

        public UserManagementUIOperations(IShowDialogAgent dialogAgent, IAddUserViewModel addUserViewModel, IAddRoleViewModel addRoleViewModel)
        {
            _dialogAgent = dialogAgent;
            _addUserViewModel = addUserViewModel;
            _addRoleViewModel = addRoleViewModel;
        }

        public UserDto AddUser()
        {
            _addUserViewModel.Initialize(new UserDto(), "Добавление нового пользователя");
            if (_dialogAgent.ShowDialog<AddUserWindow>(_addUserViewModel) == true)
            {
                return _addUserViewModel.GetUser();
            }

            return null;
        }

        public UserDto EditUser(UserViewModel user)
        {
            throw new NotImplementedException();
        }

        public RoleDto AddRole()
        {
            _addRoleViewModel.Initialize(new RoleDto(), "Добавление новой роли");
            if (_dialogAgent.ShowDialog<AddRoleWindow>(_addRoleViewModel) == false)
                return null;

            return _addRoleViewModel.GetRole();
        }

        public RoleDto EditRole(RoleViewModel role)
        {
            _addRoleViewModel.Initialize(role.ToModel(), "Редактирование роли");
            if (_dialogAgent.ShowDialog<AddRoleWindow>(_addRoleViewModel) == false)
                return null;

            return _addRoleViewModel.GetRole();
        }
    }
}