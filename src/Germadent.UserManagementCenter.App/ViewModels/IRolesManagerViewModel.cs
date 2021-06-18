using Germadent.UI.Commands;

namespace Germadent.UserManagementCenter.App.ViewModels
{
    public interface IRolesManagerViewModel
    {
        IDelegateCommand EditRoleCommand { get; }

        void Initialize();
    }
}
