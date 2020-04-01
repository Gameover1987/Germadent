using Germadent.UI.ViewModels;
using Germadent.UserManagementCenter.Model;

namespace Germadent.UserManagementCenter.App.ViewModels
{
    public class RoleViewModel : ViewModelBase
    {
        private readonly RoleDto _role;

        public RoleViewModel(RoleDto role)
        {
            _role = role;
        }

        public int RoleId => _role.RoleId;

        public string Name => _role.Name;
    }
}