using System.Linq;
using FluentAssertions;
using Germadent.Client.Common.ServiceClient;
using Germadent.Client.Common.ServiceClient.Repository;
using Germadent.Model;
using Germadent.Model.Pricing;
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
        /// Должен возвращать описание для зуба
        /// </summary>
        [TestMethod]
        [DataRow( 11,"Культя", "Полная анатомия", "ZrO2 Multicolor ST", false, "11 - Культя/ (Полная анатомия / ZrO2 Multicolor ST)")]
        public void ShouldGetCorrectDescription(int toothNumber, string prostheticsCondition, string productName, string material, bool hasBridge, string expectedDescription)
        {
            // Given
            var products = new ProductDto[]
            {
                new ProductDto
                {
                    ProductId = 1,
                    ProductName = productName,
                    MaterialId = 1,
                    MaterialName = material
                }
            };
            
            var dictionaryRepositoryMock = new Mock<IDictionaryRepository>();
            dictionaryRepositoryMock
                .Setup(x => x.Items)
                .Returns(GetProstheticConditions);
            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(x => x.Items)
                .Returns(products);
            var target = new ToothViewModel(dictionaryRepositoryMock.Object, productRepositoryMock.Object);

            var toothDto = new ToothDto
            {
                HasBridge = hasBridge,
                ToothNumber = toothNumber,
                Products = products,
                ConditionId = 1,
                ConditionName = prostheticsCondition
            };
            target.Initialize(toothDto);

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
                new DictionaryItemDto{Name = "Культя", Id = 1, Dictionary = DictionaryType.ProstheticCondition},
                new DictionaryItemDto{Name = "Имплант", Id = 2, Dictionary = DictionaryType.ProstheticCondition},
            };
            return conditions;
        }
    }
}