using Germadent.UserManagementCenter.App.ViewModels;
using Germadent.UserManagementCenter.Model;

namespace Germadent.UserManagementCenter.App.UIOperations
{
    public interface IUserManagementUIOperations
    {
        UserDto AddUser();

        UserDto EditUser(UserViewModel user);

        RoleDto AddRole();
    }
}
