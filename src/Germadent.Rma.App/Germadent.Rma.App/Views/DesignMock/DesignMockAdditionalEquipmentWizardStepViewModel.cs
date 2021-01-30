using Germadent.Rma.App.ViewModels.Wizard;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockAdditionalEquipmentWizardStepViewModel : AdditionalEquipmentWizardStepViewModel
    {
        public DesignMockAdditionalEquipmentWizardStepViewModel()
            : base(new DesignMockDictionaryRepository(), new DesignMockAttributeRepository())
        {
        }
    }
}
