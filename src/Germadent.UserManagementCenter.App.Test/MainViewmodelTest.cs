using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.UserManagementCenter.App.ViewModels;
using Moq;

namespace Germadent.UserManagementCenter.App.Test
{
    [TestFixture]
    public class MainViewModelTest
    {
        [Test]
        public void ShouldInitialize()
        {
            // Given
            var usersManager = new Mock<IUsersManagerViewModel>();
            var rolesManager = new Mock<IRolesManagerViewModel>();
            var target = new MainViewModel(usersManager.Object, rolesManager.Object);

            // When
            target.Initialize();

            // Then
            usersManager.Verify(x => x.Initialize(), Times.Once);
            rolesManager.Verify(x => x.Initialize(), Times.Once);
        }
    }
}
