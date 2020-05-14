using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.Reporting;
using Germadent.Rma.App.ViewModels.ToothCard;
using Germadent.Rma.App.ViewModels.Wizard;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockLaboratoryProjectWizardStepViewModel : LaboratoryProjectWizardStepViewModel
    {
        public DesignMockLaboratoryProjectWizardStepViewModel()
            : base(new ToothCardViewModel(new DesignMockDictionaryRepository(), new ClipboardHelper()), new DesignMockOrderFilesContainerViewModel(), new DesignMockDictionaryRepository())
        {
            var order = new OrderDto()
            {
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