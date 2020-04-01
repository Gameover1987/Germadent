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

        public WindowManager(IShowDialogAgent dialogAgent, IAddUserViewModel addUserViewModel)
        {
            _dialogAgent = dialogAgent;
            _addUserViewModel = addUserViewModel;
        }

        public UserViewModel AddUser()
        {
            _addUserViewModel.Initialize(new UserDto(), "Добавление нового пользователя");
            if (_dialogAgent.ShowDialog<AddUserWindow>(_addUserViewModel) == true)
            {
                return _addUserViewModel.GetUser();
            }

            return null;
        }

        public UserViewModel EditUser(UserViewModel user)
        {
            throw new NotImplementedException();
        }
    }
}