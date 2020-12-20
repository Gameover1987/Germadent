using Germadent.Rma.App.ViewModels.Wizard;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockMillingCenterAdditionalEquipmentWizardStepViewModel : MillingCenterAdditionalEquipmentWizardStepViewModel
    {
        public DesignMockMillingCenterAdditionalEquipmentWizardStepViewModel()
            : base(new DesignMockDictionaryRepository(), new DesignMockAttributeRepository())
        {
        }
    }
}
