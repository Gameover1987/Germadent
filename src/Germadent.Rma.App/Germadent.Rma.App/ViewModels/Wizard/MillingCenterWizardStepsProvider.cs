using Germadent.Common.CopyAndPaste;
using Germadent.Rma.App.Infrastructure;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ViewModels.ToothCard;
using Germadent.Rma.App.Views;
using Germadent.Rma.App.Views.DesignMock;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public interface IMillingCenterWizardStepsProvider : IWizardStepsProvider
    {
        void Initialize(IWindowManager windowManager);
    }

    public class MillingCenterWizardStepsProvider : IMillingCenterWizardStepsProvider
    {
        private readonly IRmaOperations _rmaOperations;
        private readonly IOrderFilesContainerViewModel _filesContainer;
        private IWindowManager _windowManager;

        public MillingCenterWizardStepsProvider(IRmaOperations rmaOperations, IOrderFilesContainerViewModel filesContainer)
        {
            _rmaOperations = rmaOperations;
            _filesContainer = filesContainer;
        }

        public IWizardStepViewModel[] GetSteps()
        {
            return new IWizardStepViewModel[]
            {
                new MillingCenterInfoWizardStepViewModel(_windowManager),
                new MillingCenterProjectWizardStepViewModel(new ToothCardViewModel(_rmaOperations, new ClipboardHelper()), _filesContainer),
                new MillingCenterAdditionalEquipmentViewModel(_rmaOperations), 
            };
        }

        public void Initialize(IWindowManager windowManager)
        {
            _windowManager = windowManager;
        }
    }
}