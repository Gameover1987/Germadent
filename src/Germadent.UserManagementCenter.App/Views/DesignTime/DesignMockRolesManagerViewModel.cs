using System.Linq;
using Germadent.UserManagementCenter.App.Mocks;
using Germadent.UserManagementCenter.App.ViewModels;

namespace Germadent.UserManagementCenter.App.Views.DesignTime
{
    public class DesignMockRolesManagerViewModel : RolesManagerViewModel
    {
        public DesignMockRolesManagerViewModel()
            : base(new DesignMockUserManagementCenterOperations(), new DesignMockWindowManager())
        {
            Initialize();
            SelectedRole = Roles.LastOrDefault();
        }
    }
}
