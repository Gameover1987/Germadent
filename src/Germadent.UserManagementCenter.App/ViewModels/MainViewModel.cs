using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Germadent.UI.ViewModels;
using Germadent.UserManagementCenter.App.ServiceClient;

namespace Germadent.UserManagementCenter.App.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        public MainViewModel(IUsersManagerViewModel usersManager)
        {
            UsersManager = usersManager;
        }

        public IUsersManagerViewModel UsersManager { get; }

        public void Initialize()
        {
            UsersManager.Initialize();
        }
    }
}
