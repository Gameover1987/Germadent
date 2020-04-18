using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.Common.Extensions;
using Germadent.Common.Logging;
using Germadent.Rma.App.Infrastructure;
using Germadent.Rma.App.Reporting;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ViewModels;
using Germadent.Rma.App.Views;
using Germadent.Rma.Model;
using Germadent.UI.Infrastructure;
using Moq;
using NUnit.Framework;

namespace Germadent.Rma.App.Test
{
    [TestFixture]
    public class MainViewModelTest
    {
        /// <summary>
        /// Должен проинициализировать вьюмодель главного окна
        /// </summary>
        [Test]
        public void ShouldInitialize()
        {
            // Given
            var mockRmaOperations = new Mock<IRmaOperations>();
            var orders = new List<OrderLiteDto>();
            orders.Add(new OrderLiteDto() { WorkOrderId = 1 });
            orders.Add(new OrderLiteDto() { WorkOrderId = 2 });
            mockRmaOperations.Setup(x => x.GetOrders(It.IsAny<OrdersFilter>())).Returns(orders.ToArray());
            var mockClipboard = new Mock<IClipboardWorks>();
            var target = new MainViewModel(mockRmaOperations.Object,
                Mock.Of<IOrderUIOperations>(),
                Mock.Of<IShowDialogAgent>(),
                Mock.Of<IPrintModule>(),
                Mock.Of<ILogger>(),
                Mock.Of<IReporter>());

            ThreadTaskExtensions.IsSyncRun = true;

            // When
            target.Initialize();

            // Then
            Assert.AreEqual(target.Orders.Count, 2);
        }
    }
}
