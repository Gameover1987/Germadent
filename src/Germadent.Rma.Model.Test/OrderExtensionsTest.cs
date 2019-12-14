using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Germadent.Rma.Model.Test
{
    [TestClass]
    public class OrderExtensionsTest
    {
        /// <summary>
        /// Должен сгенерировать правильный нмоер заказ-наряда
        /// </summary>
        [TestMethod]
        [DataRow(1, BranchType.Laboratory,"1-ЗТЛ")]
        public void ShouldGenerateOrderDocNumber(int workOrderId, BranchType branchType, string expectedDocNumber)
        {
            // Given
            var order = new OrderDto();
            order.WorkOrderId = workOrderId;
            order.BranchType = branchType;

            // When
            var actualDocNumber = order.GetOrderDocNumber();

            // Then
            Assert.AreEqual(expectedDocNumber, actualDocNumber);
        }
    }
}
