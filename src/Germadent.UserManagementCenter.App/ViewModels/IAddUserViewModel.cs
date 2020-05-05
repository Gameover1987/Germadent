using Germadent.UserManagementCenter.Model;

namespace Germadent.UserManagementCenter.App.ViewModels
{
    public interface IAddUserViewModel
    {
        void Initialize(UserDto user, string title);

        UserDto GetUser();
    }
}
