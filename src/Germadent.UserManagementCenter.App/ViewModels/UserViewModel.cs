using System.Linq;
using Germadent.UI.ViewModels;
using Germadent.UserManagementCenter.Model;

namespace Germadent.UserManagementCenter.App.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        private UserDto _user;

        public UserViewModel(UserDto user)
        {
            _user = user;
        }

        public int Id => _user.UserId;

        public string FullName => string.Format("{0} {1} {2}", _user.Surname, _user.FirstName, _user.Patronymic);

        public string Login => _user.Login;

        public string Description => _user.Description;

        public string Roles
        {
            get
            {
                if (_user.Roles == null)
                    return null;

                return string.Join(", ", _user.Roles.Select(x => x.RoleName).ToArray());
            }
        }

        public void Update(UserDto user)
        {
            _user = user;
            OnPropertyChanged();
        }

        public UserDto ToDto()
        {
            return _user;
        }
    }
}