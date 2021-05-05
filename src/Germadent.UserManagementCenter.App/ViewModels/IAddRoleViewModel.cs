using Germadent.Model;

namespace Germadent.UserManagementCenter.App.ViewModels
{
    public interface IAddRoleViewModel
    {
        void Initialize(RoleDto role, ViewMode viewMode);

        RoleDto GetRole();
    }
}