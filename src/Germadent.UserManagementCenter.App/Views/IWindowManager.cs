using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.UserManagementCenter.App.ViewModels;

namespace Germadent.UserManagementCenter.App.Views
{
    public interface IWindowManager
    {
        UserViewModel AddUser();

        UserViewModel EditUser(UserViewModel user);

        RoleViewModel AddRole();
    }
}
