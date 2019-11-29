using Germadent.Rma.App.Mocks;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public interface IMillingCenterWizardStepsProvider : IWizardStepsProvider
    {

    }

    public class MillingCenterWizardStepsProvider : IMillingCenterWizardStepsProvider
    {
        public IWizardStepViewModel[] GetSteps()
        {
            return new IWizardStepViewModel[]
            {
                new MillingCenterInfoWizardStepViewModel(),
                new MillingCenterProjectWizardStepViewModel(new MockRmaOperations()), 
            };
        }
    }
}