using NUnit.Framework;
using Germadent.TestUtils;
using Germadent.UserManagementCenter.App.ViewModels;
using Moq;

namespace Germadent.UserManagementCenter.App.Test
{
    [TestFixture]
    public class MainViewmodelTest : AutoMockerTestsBase<MainViewModel>
    {
        [Test]
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
