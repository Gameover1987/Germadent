﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.UserManagementCenter.App.Mocks;
using Germadent.UserManagementCenter.App.ServiceClient;
using Germadent.UserManagementCenter.App.ViewModels;

namespace Germadent.UserManagementCenter.App.Views.DesignTime
{
    internal class DesignMockRolesManagerViewModel : RolesManagerViewModel
    {
        public DesignMockRolesManagerViewModel()
            : base(new DesignMockUserManagementCenterOperations(), new DesignMockWindowManager())
        {
            Initialize();
            SelectedRole = Roles.LastOrDefault();
        }
    }
}
