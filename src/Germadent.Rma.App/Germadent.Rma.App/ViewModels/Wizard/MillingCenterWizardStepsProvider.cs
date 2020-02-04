using Germadent.Common.CopyAndPaste;
using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.ViewModels.ToothCard;

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
                new MillingCenterProjectWizardStepViewModel(new ToothCardViewModel(new DesignMockRmaOperations(), new ClipboardHelper())), 
            };
        }
    }
}