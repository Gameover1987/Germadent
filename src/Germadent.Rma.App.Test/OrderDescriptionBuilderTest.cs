using Germadent.Rma.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Germadent.Rma.App.Test
{
    [TestClass]
    public class OrderDescriptionBuilderTest
    {
        [TestMethod]
        [DataSource(nameof(_testCase1))]
        public void ShouldGetToothCardDescription(ToothDto[] toothCollection, string expectedDescription)
        {
            // Given

            // When
            var actualDescription = OrderDescriptionBuilder.GetToothCardDescription(toothCollection);

            // Then
            Assert.AreEqual(expectedDescription, actualDescription);
        }

        private static object[] _testCase1 =
        {
            new object[]
            {
                new[]
                {
                    CreateTooth(1, "Имплант", "Абатмент", "Ti"),
                    CreateTooth(2, "Имплант", "Абатмент", "Ti"),
                    CreateTooth(3, "Культя", "Каркас", "ZrO2"),
                    CreateTooth(4, "Культя", "Каркас", "ZrO2"),
                },
                "1, 2 - Имплант/Абатмент/Ti;   \r\n3, 4 - Культя/Каркас/ZrO2"
            },
        };

        private static ToothDto CreateTooth(int number, string prostheticCondition, string prostheticType, string material)
        {
            return new ToothDto
            {
                ToothNumber = number,
                ConditionName = prostheticCondition,
            };
        }
    }
}
