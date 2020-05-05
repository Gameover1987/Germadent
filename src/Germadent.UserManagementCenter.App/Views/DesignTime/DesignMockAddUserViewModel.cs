using System.Linq;
using Germadent.UserManagementCenter.App.Mocks;
using Germadent.UserManagementCenter.App.ViewModels;
using Germadent.UserManagementCenter.Model;

namespace Germadent.UserManagementCenter.App.Views.DesignTime
{
    public class DesignMockAddUserViewModel : AddUserViewModel
    {
        public DesignMockAddUserViewModel() 
            : base(new DesignMockUserManagementCenterOperations())
        {
            Initialize(new UserDto(), "Добавление нового пользователя");

            FullName = "Василий Алибабаевич, Вася";
            Login = "Vasya";
            Password = "123";
            PasswordOnceAgain = "123";
            Description = "Какое то описание какогото юзверя";

            Roles.First().IsChecked = true;
        }
    }
}
