using Germadent.UI.ViewModels;

namespace Germadent.UserManagementCenter.App.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        public MainViewModel(IUsersManagerViewModel usersManager, IRolesManagerViewModel rolesManager)
        {
            UsersManager = usersManager;
            RolesManager = rolesManager;
        }

        public IUsersManagerViewModel UsersManager { get; }

        public IRolesManagerViewModel RolesManager { get; }

        public void Initialize()
        {
            UsersManager.Initialize();
            RolesManager.Initialize();
        }
    }
}
