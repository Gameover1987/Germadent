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

        public string FullName => _user.FullName;

        public string Login => _user.Login;

        public string Description => _user.Description;

        public string Roles
        {
            get
            {
                if (_user.Roles == null)
                    return null;

                return string.Join(", ", _user.Roles);
            }
        }
    }
}