using Germadent.TestUtils;
using Germadent.UserManagementCenter.App.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Germadent.UserManagementCenter.App.Test
{
    [TestClass]
    public class MainViewModelTest : AutoMockerTestsBase<MainViewModel>
    {
        [TestMethod]
        public void ShouldInitialize()
        {
            // Given
            // When
            Target.Initialize();

            // Then
            GetMock<IUsersManagerViewModel>().Verify(x => x.Initialize(), Times.Once);
            GetMock<IRolesManagerViewModel>().Verify(x => x.Initialize(), Times.Once);
        }
    }
}
