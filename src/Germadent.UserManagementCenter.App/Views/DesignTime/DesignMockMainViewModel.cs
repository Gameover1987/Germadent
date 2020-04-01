using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.UserManagementCenter.App.ViewModels;

namespace Germadent.UserManagementCenter.App.Views.DesignTime
{
    public class DesignMockMainViewModel : MainViewModel
    {
        public DesignMockMainViewModel() 
            : base(new DesignMockUsersManagerViewModel(), new DesignMockRolesManagerViewModel())
        {
            Initialize();
        }
    }
}
