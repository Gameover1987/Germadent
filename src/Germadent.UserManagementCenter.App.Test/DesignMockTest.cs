using Germadent.UserManagementCenter.App.Views.DesignTime;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Germadent.UserManagementCenter.App.Test
{
    [TestClass]
    public class DesignMockTest
    {
        [TestMethod]
        public void ShouldCreateAllDesignMockViewModels()
        {
            var designMockRolesManagerViewModel = new DesignMockRolesManagerViewModel();
        }
    }
}
