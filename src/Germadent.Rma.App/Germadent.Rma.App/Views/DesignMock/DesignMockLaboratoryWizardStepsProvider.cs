using Germadent.Rma.App.ViewModels.Wizard;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockLaboratoryWizardStepsProvider : IWizardStepsProvider
    {
        public BranchType BranchType
        {
            get { return BranchType.Laboratory; }
        }

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