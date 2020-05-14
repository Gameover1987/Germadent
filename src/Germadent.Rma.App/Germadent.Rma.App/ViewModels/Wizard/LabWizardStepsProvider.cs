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
        private readonly IDictionaryRepository _dictionaryRepository;
        private readonly IOrderFilesContainerViewModel _filesContainer;

        public LabWizardStepsProvider(IDictionaryRepository dictionaryRepository, IOrderFilesContainerViewModel filesContainer)
        {
            _dictionaryRepository = dictionaryRepository;
            _filesContainer = filesContainer;
        }

        public BranchType BranchType => BranchType.Laboratory;

        public IWizardStepViewModel[] GetSteps()
        {
            return new IWizardStepViewModel[]
            {
                new LaboratoryInfoWizardStepViewModel(),
                new LaboratoryProjectWizardStepViewModel(new ToothCardViewModel(_dictionaryRepository, new ClipboardHelper()), _filesContainer, _dictionaryRepository),
            };
        }
    }
}