using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.Reporting;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.App.ViewModels.ToothCard;
using Germadent.Rma.App.Views.DesignMock;
using Germadent.Rma.Model;

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

        public BranchType BranchType => BranchType.Laboratory;

        public IWizardStepViewModel[] GetSteps()
        {
            return new IWizardStepViewModel[]
            {
                new LaboratoryInfoWizardStepViewModel(),
                new LaboratoryProjectWizardStepViewModel(new ToothCardViewModel(new DesignMockDictionaryRepository(), new ClipboardHelper()), _filesContainer, new DesignMockDictionaryRepository()),
            };
        }
    }
}