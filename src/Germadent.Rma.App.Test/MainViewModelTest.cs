using System.Collections.Generic;
using Germadent.Client.Common.Infrastructure;
using Germadent.Client.Common.Reporting;
using Germadent.Client.Common.ServiceClient.Notifications;
using Germadent.Common.Extensions;
using Germadent.Common.Logging;
using Germadent.Model;
using Germadent.Rma.App.Infrastructure;
using Germadent.Rma.App.Operations;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ViewModels;
using Germadent.Rma.App.ViewModels.Pricing;
using Germadent.Rma.App.ViewModels.TechnologyOperation;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;
using Germadent.UI.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Germadent.Rma.App.Test
{
    [TestClass]
    public class MainViewModelTest
    {
        /// <summary>
        /// Должен проинициализировать вьюмодель главного окна
        /// </summary>
        [TestMethod]
        public void ShouldInitialize()
        {
            // Given
            var mockRmaOperations = new Mock<IRmaServiceClient>();
            var orders = new List<OrderLiteDto>();
            orders.Add(new OrderLiteDto() { WorkOrderId = 1 });
            orders.Add(new OrderLiteDto() { WorkOrderId = 2 });
            mockRmaOperations.Setup(x => x.GetOrders(It.IsAny<OrdersFilter>())).Returns(orders.ToArray());
            var mockClipboard = new Mock<IClipboardHelper>();
            var target = new MainViewModel(mockRmaOperations.Object,
                Mock.Of<IEnvironment>(),
                Mock.Of<IOrderUIOperations>(),
                Mock.Of<IShowDialogAgent>(),
                Mock.Of<ICustomerCatalogViewModel>(),
                Mock.Of<IResponsiblePersonCatalogViewModel>(),
                Mock.Of<IPriceListEditorContainerViewModel>(),
                Mock.Of<ITechnologyOperationsEditorViewModel>(),
                Mock.Of<IPrintModule>(),
                Mock.Of<ILogger>(),
                Mock.Of<IUserManager>(),
                Mock.Of<IUserSettingsManager>(),
                Mock.Of<IClipboardHelper>(),
                Mock.Of<ISignalRClient>());

            ThreadTaskExtensions.IsSyncRun = true;

            // When
            target.Initialize();

            // Then
            Assert.AreEqual(target.Orders.Count, 2);
        }
    }
}
