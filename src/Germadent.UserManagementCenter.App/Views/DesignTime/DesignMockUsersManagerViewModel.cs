using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.UI.Infrastructure;
using Germadent.UserManagementCenter.App.ServiceClient;
using Germadent.UserManagementCenter.App.ViewModels;

namespace Germadent.UserManagementCenter.App.Views.DesignTime
{
    internal class DesignMockUsersManagerViewModel : UsersManagerViewModel
    {
        public DesignMockUsersManagerViewModel() : base(new UserManagementCenterOperations(), new DesignMockWindowManager() )
        {
            Initialize();
            SelectedUser = Users.LastOrDefault();
        }
    }

    internal class DesignMockWindowManager : IWindowManager
    {
        public UserViewModel AddUser()
        {
            throw new NotImplementedException();
        }

        public UserViewModel EditUser(UserViewModel user)
        {
            throw new NotImplementedException();
        }
    }
}
