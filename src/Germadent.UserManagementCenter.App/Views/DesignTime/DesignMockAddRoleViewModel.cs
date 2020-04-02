using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.Common.Extensions;
using Germadent.UserManagementCenter.App.ServiceClient;
using Germadent.UserManagementCenter.App.ViewModels;
using Germadent.UserManagementCenter.Model;

namespace Germadent.UserManagementCenter.App.Views.DesignTime
{
    internal class DesignMockAddRoleViewModel : AddRoleViewModel
    {
        public DesignMockAddRoleViewModel()
            : base(new UserManagementCenterOperations())
        {
            Initialize(new RoleDto(), "Добавление новой роли");

            RoleName = "Мега администратор!";
            Rights.ForEach(x => x.IsChecked = true);
        }
    }
}
