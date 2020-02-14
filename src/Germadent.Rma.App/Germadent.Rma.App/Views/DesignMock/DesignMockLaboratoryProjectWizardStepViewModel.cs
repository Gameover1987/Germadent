using Germadent.Common.CopyAndPaste;
using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.ViewModels.ToothCard;
using Germadent.Rma.App.ViewModels.Wizard;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockLaboratoryProjectWizardStepViewModel : LaboratoryProjectWizardStepViewModel
    {
        public DesignMockLaboratoryProjectWizardStepViewModel()
            : base(new ToothCardViewModel(new DesignMockRmaOperations(), new ClipboardHelper()), new DesignMockOrderFilesContainerViewModel(), new DesignMockRmaOperations())
        {
            var order = new OrderDto()
            {
                WorkDescription = "Очень много разной интересной работы, делать ее родиму не переделать. Но мы же крутые спецы, сделаем! Очень много разной интересной работы, делать ее родиму не переделать. Но мы же крутые спецы, сделаем Но мы же крутые спецы, сделаем! Очень много разной интересной работы, делать ее родиму не переделать. Но мы же крутые спецы, сделаем!",
                ColorAndFeatures = "Белоснежный",
                ToothCard = CreateMockToothCard(),
                Transparency = 2,
                ProstheticArticul = "Какой то артикул"
            };
            Initialize(order);
        }

        private static ToothDto[] CreateMockToothCard()
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