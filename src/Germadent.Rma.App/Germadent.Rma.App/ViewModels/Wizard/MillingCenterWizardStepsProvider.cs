using Germadent.Rma.App.Infrastructure;
using Germadent.Rma.App.Reporting;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.App.ViewModels.ToothCard;
using Germadent.Rma.Model;
using Germadent.UI.Controls;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public interface IMillingCenterWizardStepsProvider : IWizardStepsProvider
    {
    }

    public class MillingCenterWizardStepsProvider : IMillingCenterWizardStepsProvider
    {
        private readonly IRmaServiceClient _rmaOperations;
        private readonly IOrderFilesContainerViewModel _filesContainer;
        private readonly ISuggestionProvider _customerSuggestionProvider;
        private readonly ICatalogUIOperations _catalogUIOperations;
        private readonly ICustomerRepository _customerRepository;

        public MillingCenterWizardStepsProvider(IRmaServiceClient rmaOperations, 
            IOrderFilesContainerViewModel filesContainer, 
            ISuggestionProvider customerSuggestionProvider, 
            ISuggestionProvider responsiblePersonSuggestionProvider, 
            ICatalogUIOperations catalogUIOperations,
            ICustomerRepository customerRepository)
        {
            _rmaOperations = rmaOperations;
            _filesContainer = filesContainer;
            _customerSuggestionProvider = customerSuggestionProvider;
            _catalogUIOperations = catalogUIOperations;
            _customerRepository = customerRepository;
        }

        public BranchType BranchType => BranchType.MillingCenter;

        public IWizardStepViewModel[] GetSteps()
        {
            return new IWizardStepViewModel[]
            {
                new MillingCenterInfoWizardStepViewModel(_catalogUIOperations, _customerSuggestionProvider, _customerRepository),
                new MillingCenterProjectWizardStepViewModel(new ToothCardViewModel(_rmaOperations, new ClipboardHelper()), _filesContainer),
                new MillingCenterAdditionalEquipmentViewModel(_rmaOperations), 
            };
        }
    }
}