using Germadent.Common.Extensions;
using Germadent.UserManagementCenter.App.Mocks;
using Germadent.UserManagementCenter.App.ViewModels;
using Germadent.UserManagementCenter.Model;

namespace Germadent.UserManagementCenter.App.Views.DesignTime
{
    internal class DesignMockAddRoleViewModel : AddRoleViewModel
    {
        public DesignMockAddRoleViewModel()
            : base(new DesignMockUserManagementCenterOperations())
        {
            Initialize(new RoleDto(), ViewMode.Add);

            RoleName = "Мега администратор!";
            Rights.ForEach(x => x.IsChecked = true);
        }
    }
}
