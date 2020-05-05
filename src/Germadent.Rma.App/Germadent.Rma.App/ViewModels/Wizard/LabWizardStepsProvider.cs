using Germadent.Rma.App.Reporting;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ViewModels.ToothCard;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public interface ILabWizardStepsProvider : IWizardStepsProvider
    {

    }

    public class LabWizardStepsProvider : ILabWizardStepsProvider
    {
        private readonly IRmaServiceClient _rmaOperations;
        private readonly IOrderFilesContainerViewModel _filesContainer;

        public LabWizardStepsProvider(IRmaServiceClient rmaOperations, IOrderFilesContainerViewModel filesContainer)
        {
            _rmaOperations = rmaOperations;
            _filesContainer = filesContainer;
        }

        public IWizardStepViewModel[] GetSteps()
        {
            return new IWizardStepViewModel[]
            {
                new LaboratoryInfoWizardStepViewModel(),
                new LaboratoryProjectWizardStepViewModel(new ToothCardViewModel(_rmaOperations, new ClipboardHelper()), _filesContainer, _rmaOperations),
            };
        }
    }
}