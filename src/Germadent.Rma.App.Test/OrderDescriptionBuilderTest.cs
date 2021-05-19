using Germadent.Model;
using Germadent.Model.Pricing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Germadent.Rma.App.Test
{
    [TestClass]
    public class OrderDescriptionBuilderTest
    {
        [TestMethod]
        public void ShouldGetToothCardDescription()
        {
            // Given

            // When
            var actualDescription = OrderDescriptionBuilder.GetToothCardDescription(DescriptionTestData.Teeth);

            // Then
            Assert.AreEqual(DescriptionTestData.ExpectedDescription, actualDescription);
        }

        private OrderDescriptionTestData DescriptionTestData
        {
            get
            {
                return new OrderDescriptionTestData
                {
                    Teeth = new[]
                    {
                        CreateTooth(1, "Имплант", "Абатмент", "Ti"),
                        CreateTooth(2, "Имплант", "Абатмент", "Ti"),
                        CreateTooth(3, "Культя", "Каркас", "ZrO2"),
                        CreateTooth(4, "Культя", "Каркас", "ZrO2"),
                    },
                    ExpectedDescription = "1, 2 - Имплант/ (Абатмент / Ti);   \r\n3, 4 - Культя/ (Каркас / ZrO2)"
                };
            }
        }

        private static ToothDto CreateTooth(int number, string prostheticCondition, string productName, string material)
        {
            return new ToothDto
            {
                ToothNumber = number,
                ConditionName = prostheticCondition,
                Products = new[]
                {
                    new ProductDto
                    {
                        ProductName = productName,
                        MaterialName = material
                    },
                }
            };
        }
    }

    public class OrderDescriptionTestData
    {
        public ToothDto[] Teeth { get; set; }

        public string ExpectedDescription { get; set; } 
    }
}
