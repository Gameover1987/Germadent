using Germadent.Rma.App.Reporting;
using Germadent.Rma.App.ViewModels.ToothCard;
using Germadent.Rma.Model;
using Moq;
using System.Linq;
using Germadent.Rma.App.ServiceClient.Repository;
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
            target.RenderRequest += (sender, args) => { raised = true; };

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

            return new ToothCardViewModel(mockDictionaryRepository.Object, Mock.Of<IClipboardHelper>());
        }

        private static ToothDto[] CreateToothCard()
        {
            return new ToothDto[]
            {
                new ToothDto
                {
                    HasBridge = true,
                    MaterialId = 1,
                    MaterialName = "ZrO",
                    ProstheticsId = 1,
                    ProstheticsName = "Каркас",
                    ToothNumber = 11
                },
                new ToothDto
                {
                    HasBridge = true,
                    MaterialId = 1,
                    MaterialName = "ZrO",
                    ProstheticsId = 1,
                    ProstheticsName = "Каркас",
                    ToothNumber = 12
                },
            };
        }
    }
}
