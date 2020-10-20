using Germadent.UserManagementCenter.App.ViewModels;
using Germadent.UserManagementCenter.Model;

namespace Germadent.UserManagementCenter.App.UIOperations
{
    public interface IUserManagementUIOperations
    {
        UserDto AddUser();

        UserDto EditUser(UserDto user);

        RoleDto AddRole();

        RoleDto EditRole(RoleViewModel role);

        void DeleteRole(RoleViewModel role);
    }
}
