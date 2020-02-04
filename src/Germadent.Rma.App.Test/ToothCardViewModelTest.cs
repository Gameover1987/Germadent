using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.Common.CopyAndPaste;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ViewModels.ToothCard;
using Germadent.Rma.Model;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Germadent.Rma.App.Test
{
    [TestFixture]
    public class ToothCardViewModelTest
    {
        /// <summary>
        /// Должен перерисовать мост когда меняется флаг для соответствующего зуба
        /// </summary>
        [Test]
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
            var rmaOperationsMock = new Mock<IRmaOperations>();
            rmaOperationsMock
                .Setup(x => x.GetMaterials())
                .Returns(new[]
                {
                    new MaterialDto {Name = "ZrO", Id = 1},
                });

            rmaOperationsMock
                .Setup(x => x.GetProstheticTypes())
                .Returns(new ProstheticsTypeDto[]
                {
                    new ProstheticsTypeDto {Name = "Каркас", Id = 1},

                });

            return new ToothCardViewModel(rmaOperationsMock.Object, Mock.Of<IClipboardHelper>());
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
