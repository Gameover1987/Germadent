using System;
using System.Linq;
using Germadent.UserManagementCenter.App.Mocks;
using Germadent.UserManagementCenter.App.UIOperations;
using Germadent.UserManagementCenter.App.ViewModels;
using Germadent.UserManagementCenter.Model;

namespace Germadent.UserManagementCenter.App.Views.DesignTime
{
    internal class DesignMockUsersManagerViewModel : UsersManagerViewModel
    {
        public DesignMockUsersManagerViewModel() : base(new DesignMockUserManagementCenterOperations(), new DesignMockWindowManager() )
        {
            Initialize();
            SelectedUser = Users.LastOrDefault();
        }
    }

    internal class DesignMockWindowManager : IUserManagementUIOperations
    {
        public UserDto AddUser()
        {
            throw new NotImplementedException();
        }

        public UserDto EditUser(UserDto user)
        {
            throw new NotImplementedException();
        }

        public RoleDto AddRole()
        {
            throw new NotImplementedException();
        }

        public RoleDto EditRole(RoleViewModel role)
        {
            throw new NotImplementedException();
        }
    }
}
