using Germadent.UI.ViewModels;
using Germadent.UserManagementCenter.Model;

namespace Germadent.UserManagementCenter.App.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        private readonly UserDto _user;

        public UserViewModel(UserDto user)
        {
            _user = user;
        }
    }
}