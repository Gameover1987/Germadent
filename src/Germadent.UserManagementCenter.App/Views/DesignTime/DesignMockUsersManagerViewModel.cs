using System;
using System.Linq;
using Germadent.Model;
using Germadent.UserManagementCenter.App.Mocks;
using Germadent.UserManagementCenter.App.UIOperations;
using Germadent.UserManagementCenter.App.ViewModels;

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

        public bool DeleteUser(UserViewModel user)
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

        public bool DeleteRole(RoleViewModel role)
        {
            throw new NotImplementedException();
        }
    }
}
