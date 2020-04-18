using Germadent.Common.CopyAndPaste;
using Germadent.Rma.App.Infrastructure;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ViewModels.ToothCard;
using Germadent.Rma.App.Views;
using Germadent.Rma.App.Views.DesignMock;
using Germadent.UI.Controls;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public interface IMillingCenterWizardStepsProvider : IWizardStepsProvider
    {
    }

    public class MillingCenterWizardStepsProvider : IMillingCenterWizardStepsProvider
    {
        private readonly IRmaOperations _rmaOperations;
        private readonly IOrderFilesContainerViewModel _filesContainer;
        private readonly ISuggestionProvider _customerSuggestionProvider;
        private readonly ICatalogUIOperations _catalogUIOperations;

        public MillingCenterWizardStepsProvider(IRmaOperations rmaOperations, 
            IOrderFilesContainerViewModel filesContainer, 
            ISuggestionProvider customerSuggestionProvider, 
            ISuggestionProvider responsiblePersonSuggestionProvider, 
            ICatalogUIOperations catalogUIOperations)
        {
            _rmaOperations = rmaOperations;
            _filesContainer = filesContainer;
            _customerSuggestionProvider = customerSuggestionProvider;
            _catalogUIOperations = catalogUIOperations;
        }

        public IWizardStepViewModel[] GetSteps()
        {
            return new IWizardStepViewModel[]
            {
                new MillingCenterInfoWizardStepViewModel(_catalogUIOperations, _customerSuggestionProvider),
                new MillingCenterProjectWizardStepViewModel(new ToothCardViewModel(_rmaOperations, new ClipboardHelper()), _filesContainer),
                new MillingCenterAdditionalEquipmentViewModel(_rmaOperations), 
            };
        }
    }
}