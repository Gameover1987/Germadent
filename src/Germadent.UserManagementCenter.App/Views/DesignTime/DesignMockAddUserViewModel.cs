using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.Common.Extensions;
using Germadent.UserManagementCenter.App.ServiceClient;
using Germadent.UserManagementCenter.App.ViewModels;
using Germadent.UserManagementCenter.Model;

namespace Germadent.UserManagementCenter.App.Views.DesignTime
{
    public class DesignMockAddUserViewModel : AddUserViewModel
    {
        public DesignMockAddUserViewModel() 
            : base(new UserManagementCenterOperations())
        {
            Initialize(new UserDto(), "Добавление нового пользователя");

            FullName = "Василий Алибабаевич, Вася";
            Login = "Vasya";
            Password = "123";
            PasswordOnceAgain = "123";

            Roles.First().IsChecked = true;
        }
    }
}
