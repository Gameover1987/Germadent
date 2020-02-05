using Germadent.Common.CopyAndPaste;
using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ViewModels.ToothCard;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public interface ILabWizardStepsProvider : IWizardStepsProvider
    {

    }

    public class LabWizardStepsProvider : ILabWizardStepsProvider
    {
        private readonly IRmaOperations _rmaOperations;

        public LabWizardStepsProvider(IRmaOperations rmaOperations)
        {
            _rmaOperations = rmaOperations;
        }

        public IWizardStepViewModel[] GetSteps()
        {
            return new IWizardStepViewModel[]
            {
                new LaboratoryInfoWizardStepViewModel(),
                new LaboratoryProjectWizardStepViewModel(new ToothCardViewModel(_rmaOperations, new ClipboardHelper())),
            };
        }
    }
}