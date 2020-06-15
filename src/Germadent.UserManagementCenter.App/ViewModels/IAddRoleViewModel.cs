using Germadent.UserManagementCenter.Model;

namespace Germadent.UserManagementCenter.App.ViewModels
{
    public interface IAddRoleViewModel
    {
        void Initialize(RoleDto role, string title);

        RoleDto GetRole();
    }
}