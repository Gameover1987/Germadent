using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.UserManagementCenter.Model;

namespace Germadent.UserManagementCenter.App.ViewModels
{
    public interface IAddUserViewModel
    {
        void Initialize(UserDto userDto, string title);

        UserDto GetUser();
    }
}
