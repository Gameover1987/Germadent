using Germadent.Model.Rights;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Germadent.UserManagementCenter.App.Test
{
    [TestClass]
    public class UserRightsProviderTest
    {
        [TestMethod]
        public void Test()
        {
            var allUserRights = UserRightsProvider.GetAllUserRights();
        }
    }
}
