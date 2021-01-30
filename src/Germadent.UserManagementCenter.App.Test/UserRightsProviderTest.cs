using System;
using System.Collections.Generic;
using System.Text;
using Germadent.UserManagementCenter.Model.Rights;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Germadent.UserManagementCenter.App.Test
{
    [TestClass]
    public class UserRightsProviderTest
    {
        [TestMethod]
        public void Test()
        {
            var aaa = UserRightsProvider.GetAllUserRights();
        }
    }
}
