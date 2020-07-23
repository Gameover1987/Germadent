using System;
using System.Collections.Generic;
using System.Text;
using Germadent.Rma.Model;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Germadent.Rma.App.Test
{
    [TestFixture]
    public class OrderDescriptionBuilderTest
    {
        [TestCaseSource(nameof(_testCase1))]
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
                    CreateTooth( 1, "Абатмент", "Имплант", "CrO"),
                    CreateTooth( 2, "Абатмент", "Имплант", "CrO"),
                },
                "1,2 - Имплант/Абатмент/CrO"
            },
        };
     

        private static ToothDto CreateTooth(int number, string prostheticType, string prostheticCondition, string material)
        {
            return new ToothDto
            {
                ToothNumber = number,
                ConditionName = prostheticCondition,
                ProstheticsName = prostheticType,
                MaterialName = material
            };
        }
    }
}
