using Germadent.Rma.Model;
using System.Linq;
using FluentAssertions;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.App.ViewModels.ToothCard;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Germadent.Rma.App.Test
{
    [TestClass]
    public class ToothViewModelTest
    {
        /// <summary>
        /// Должен инициализироваться из DTO
        /// </summary>
        /// <param name="hasBridge"></param>
        /// <param name="material"></param>
        /// <param name="productType"></param>
        [TestMethod]
        [DataRow(true, "ZrO", "Каркас")]
        [DataRow(false, "E.MAX", "другая конструкция")]
        public void ShouldInitializeFromDto(bool hasBridge, string material, string productType)
        {
            // Given
            var target = CreateTarget();

            // When
            target.Initialize(new ToothDto
            {
                HasBridge = hasBridge,
            });

            // Then
            Assert.AreEqual(hasBridge, target.HasBridge);
        }

        [TestMethod]
        [DataRow(10, 1, "ZrO", 1, "Каркас", true)]
        public void ShouldGetDto(int toothNumber, bool hasBridge)
        {
            // Given
            var expectedDto = new ToothDto
            {
                HasBridge = hasBridge,
                ToothNumber = toothNumber
            };
            var target = CreateTarget();
            target.Initialize(expectedDto);

            // When
            var actualDto = target.ToDto();

            // Then
            actualDto.Should().BeEquivalentTo(expectedDto);
        }

        /// <summary>
        /// Должен возвращать описание для зуба
        /// </summary>
        [TestMethod]
        [DataRow("Культя", "Каркас", "ZrO", true, "0 - Культя/Каркас/ZrO/Мост")]
        [DataRow(null, "Каркас", "ZrO", true, "0 - Каркас/ZrO/Мост")]
        [DataRow(null, null, "ZrO", true, "0 - ZrO/Мост")]
        [DataRow(null, null, null, true, "0 - Мост")]
        public void ShouldGetCorrectDescription(string prostheticsCondition, string prosthetics, string material, bool hasBridge, string expectedDescription)
        {
            // Given
            var target = CreateTarget();
            var selectedProstheticCondition = target.ProstheticConditions.FirstOrDefault(x => x.DisplayName == prostheticsCondition);
            if (selectedProstheticCondition != null)
                selectedProstheticCondition.IsChecked = true;

            target.HasBridge = hasBridge;

            // When
            var actualDescription = target.Description;

            // Then
            Assert.AreEqual(expectedDescription, actualDescription);
        }

        private ToothViewModel CreateTarget()
        {
            var dictionaryRepositoryMock = new Mock<IDictionaryRepository>();
            dictionaryRepositoryMock.Setup(x => x.GetItems(It.IsAny<DictionaryType>()))
                .Returns(GetProstheticConditions);

            return new ToothViewModel(dictionaryRepositoryMock.Object, Mock.Of<IProductRepository>());
        }

        private static DictionaryItemDto[] GetProstheticConditions()
        {
            var conditions = new[]
            {
                new DictionaryItemDto{Name = "Культя", Id = 1},
                new DictionaryItemDto{Name = "Имплант", Id = 2},
            };
            return conditions;
        }
    }
}