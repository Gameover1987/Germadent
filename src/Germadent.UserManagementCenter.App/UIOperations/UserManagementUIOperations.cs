﻿using System.Windows;
using Germadent.Model;
using Germadent.UI.Infrastructure;
using Germadent.UserManagementCenter.App.ServiceClient;
using Germadent.UserManagementCenter.App.ViewModels;
using Germadent.UserManagementCenter.App.Views;

namespace Germadent.UserManagementCenter.App.UIOperations
{
    public sealed class UserManagementUIOperations : IUserManagementUIOperations
    {
        private readonly IUmcServiceClient _umcServiceClient;
        private readonly IShowDialogAgent _dialogAgent;
        private readonly IAddUserViewModel _addUserViewModel;
        private readonly IAddRoleViewModel _addRoleViewModel;

        public UserManagementUIOperations(IUmcServiceClient umcServiceClient, IShowDialogAgent dialogAgent, IAddUserViewModel addUserViewModel, IAddRoleViewModel addRoleViewModel)
        {
            _umcServiceClient = umcServiceClient;
            _dialogAgent = dialogAgent;
            _addUserViewModel = addUserViewModel;
            _addRoleViewModel = addRoleViewModel;
        }

        public UserDto AddUser()
        {
            _addUserViewModel.Initialize(new UserDto(), ViewMode.Add);
            if (_dialogAgent.ShowDialog<AddUserWindow>(_addUserViewModel) == false)
                return null;

            return _addUserViewModel.GetUser();
        }

        public UserDto EditUser(UserDto user)
        {
            _addUserViewModel.Initialize(user, ViewMode.Edit);
            if (_dialogAgent.ShowDialog<AddUserWindow>(_addUserViewModel) == false)
                return null;

            return _addUserViewModel.GetUser();
        }

        public bool DeleteUser(UserViewModel user)
        {
            if (_dialogAgent.ShowMessageDialog(
                    string.Format("Вы действительно хотите удалить пользователя '{0}'", user.FullName),
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return false;

            _umcServiceClient.DeleteUser(user.Id);
            return true;
        }

        public RoleDto AddRole()
        {
            _addRoleViewModel.Initialize(new RoleDto(), ViewMode.Add);
            if (_dialogAgent.ShowDialog<AddRoleWindow>(_addRoleViewModel) == false)
                return null;

            return _addRoleViewModel.GetRole();
        }

        public RoleDto EditRole(RoleViewModel role)
        {
            _addRoleViewModel.Initialize(role.ToModel(), ViewMode.Edit);
            if (_dialogAgent.ShowDialog<AddRoleWindow>(_addRoleViewModel) == false)
                return null;

            return _addRoleViewModel.GetRole();
        }

        public bool DeleteRole(RoleViewModel role)
        {
            if (_dialogAgent.ShowMessageDialog(string.Format("Вы действительно хотите удалить роль '{0}'", role.Name),
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return false;

            _umcServiceClient.DeleteRole(role.RoleId);
            return true;
        }
    }
}