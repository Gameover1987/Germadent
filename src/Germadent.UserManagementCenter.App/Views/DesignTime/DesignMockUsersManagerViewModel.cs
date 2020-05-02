using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.UI.Infrastructure;
using Germadent.UserManagementCenter.App.Mocks;
using Germadent.UserManagementCenter.App.ServiceClient;
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

        public UserDto EditUser(UserViewModel user)
        {
            throw new NotImplementedException();
        }

        public RoleDto AddRole()
        {
            throw new NotImplementedException();
        }
    }
}
