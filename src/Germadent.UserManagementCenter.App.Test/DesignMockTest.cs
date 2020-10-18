using System;
using System.Collections.Generic;
using System.Text;
using Germadent.UserManagementCenter.App.Views.DesignTime;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Germadent.UserManagementCenter.App.Test
{
    [TestFixture]
    public class DesignMockTest
    {
        [Test]
        public void ShouldCreateAllDesignMockViewModels()
        {
            var designMockRolesManagerViewModel = new DesignMockRolesManagerViewModel();
        }
    }
}
