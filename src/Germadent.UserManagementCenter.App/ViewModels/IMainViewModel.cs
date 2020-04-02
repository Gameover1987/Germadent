namespace Germadent.UserManagementCenter.App.ViewModels
{
    public interface IMainViewModel
    {
        IUsersManagerViewModel UsersManager { get; }

        IRolesManagerViewModel RolesManager { get; }

        void Initialize();
    }
}