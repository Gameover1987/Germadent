using Germadent.UserManagementCenter.App.ViewModels;
using Germadent.UserManagementCenter.Model;

namespace Germadent.UserManagementCenter.App.UIOperations
{
    public interface IUserManagementUIOperations
    {
        UserDto AddUser();

        UserDto EditUser(UserDto user);

        bool DeleteUser(UserViewModel user);

        RoleDto AddRole();

        RoleDto EditRole(RoleViewModel role);

        bool DeleteRole(RoleViewModel role);
    }
}
