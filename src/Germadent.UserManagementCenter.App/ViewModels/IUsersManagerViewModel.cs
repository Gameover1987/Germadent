using Germadent.UI.Commands;

namespace Germadent.UserManagementCenter.App.ViewModels
{
    public interface IUsersManagerViewModel
    {
        IDelegateCommand EditUSerCommand { get; }

        void Initialize();
    }
}