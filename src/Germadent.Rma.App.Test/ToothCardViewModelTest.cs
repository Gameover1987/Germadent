using Moq;
using System.Linq;
using Germadent.Client.Common.Infrastructure;
using Germadent.Client.Common.ServiceClient;
using Germadent.Client.Common.ServiceClient.Repository;
using Germadent.Model;
using Germadent.Model.Pricing;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.App.ViewModels.ToothCard;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Germadent.Rma.App.Test
{
    [TestClass]
    public class ToothCardViewModelTest
    {
        /// <summary>
        /// Должен перерисовать мост когда меняется флаг для соответствующего зуба
        /// </summary>
        [TestMethod]
        public void ShouldRaiseRenderRequestEventWhenHasBridgeChanged()
        {
            // Given
            var raised = false;
            var target = CreateTarget();
            target.ToothChanged += (sender, args) => { raised = true; };

            // When
            target.Initialize(CreateToothCard());
            target.Teeth.First().HasBridge = true;

            // Then
            Assert.IsTrue(raised);
        }

        private ToothCardViewModel CreateTarget()
        {
            var mockDictionaryRepository = new Mock<IDictionaryRepository>();
            mockDictionaryRepository
                .Setup(x => x.GetItems(DictionaryType.Material))
                .Returns(new[]
                {
                    new DictionaryItemDto {Name = "ZrO", Id = 1},
                });

            mockDictionaryRepository
                .Setup(x => x.GetItems(DictionaryType.ProstheticType))
                .Returns(new DictionaryItemDto[]
                {
                    new DictionaryItemDto {Name = "Каркас", Id = 1},

                });

            return new ToothCardViewModel(mockDictionaryRepository.Object, Mock.Of<IProductRepository>(), Mock.Of<IClipboardHelper>());
        }

        private static ToothDto[] CreateToothCard()
        {
            return new ToothDto[]
            {
                new ToothDto
                {
                    HasBridge = true,
                    Products = new ProductDto[0],
                    ToothNumber = 11
                },
                new ToothDto
                {
                    HasBridge = true,
                    Products = new ProductDto[0],
                    ToothNumber = 12
                },
            };
        }
    }
}
