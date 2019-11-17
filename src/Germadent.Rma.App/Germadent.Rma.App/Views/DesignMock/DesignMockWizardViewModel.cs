using Germadent.Rma.App.ViewModels.Wizard;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockWizardViewModel : WizardViewModel
    {
        public DesignMockWizardViewModel() 
            : base(new DesignMockLaboratoryWizardStepsProvider())
        {
        }
    }

    public class DesignMockLaboratoryWizardStepsProvider : IWizardStepsProvider
    {
        public IWizardStepViewModel[] GetSteps()
        {
            return new IWizardStepViewModel[]
            {
                new DesignMockLaboratoryInfoWizardStepViewModel(),
                new DesignMockLaboratoryProjectWizardStepViewModel(),
            };
        }
    }
}
