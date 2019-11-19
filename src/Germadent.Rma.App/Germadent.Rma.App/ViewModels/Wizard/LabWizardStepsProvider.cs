using Germadent.Rma.App.Mocks;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public interface ILabWizardStepsProvider : IWizardStepsProvider
    {

    }

    public class LabWizardStepsProvider : ILabWizardStepsProvider
    {
        public IWizardStepViewModel[] GetSteps()
        {
            return new IWizardStepViewModel[]
            {
                new LaboratoryInfoWizardStepViewModel(),
                new LaboratoryProjectWizardStepViewModel(new MockRmaOperations()),
            };
        }
    }
}