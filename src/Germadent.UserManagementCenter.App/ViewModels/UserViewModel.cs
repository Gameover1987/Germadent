using System.Linq;
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

        public string Login => _user.Login;

        public string Description => _user.Description;

        public string Roles
        {
            get { return string.Join(", ", _user.Roles); }
        }
    }
}