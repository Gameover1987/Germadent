using Germadent.Rma.App.Infrastructure;
using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.Operations;
using Germadent.Rma.App.Reporting;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.App.ViewModels.ToothCard;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;
using Germadent.Rma.App.Views.DesignMock;
using Germadent.Rma.Model;
using Germadent.UI.Controls;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public interface IMillingCenterWizardStepsProvider : IWizardStepsProvider
    {
    }

    public class MillingCenterWizardStepsProvider : IMillingCenterWizardStepsProvider
    {
        private readonly IRmaServiceClient _rmaServiceClient;
        private readonly IOrderFilesContainerViewModel _filesContainer;
        private readonly ICustomerSuggestionProvider _customerSuggestionProvider;
        private readonly IResponsiblePersonsSuggestionsProvider _responsiblePersonSuggestionProvider;
        private readonly ICatalogUIOperations _catalogUIOperations;
        private readonly ICatalogSelectionUIOperations _catalogSelectionOperations;
        private readonly ICustomerRepository _customerRepository;
        private readonly IResponsiblePersonRepository _responsiblePersonRepository;
        private readonly IDictionaryRepository _dictionaryRepository;

        public MillingCenterWizardStepsProvider(IRmaServiceClient rmaServiceClient,
            IOrderFilesContainerViewModel filesContainer,
            ICustomerSuggestionProvider customerSuggestionProvider,
            IResponsiblePersonsSuggestionsProvider responsiblePersonSuggestionProvider,
            ICatalogUIOperations catalogUIOperations,
            ICatalogSelectionUIOperations catalogSelectionOperations,
            ICustomerRepository customerRepository,
            IResponsiblePersonRepository responsiblePersonRepository,
            IDictionaryRepository dictionaryRepository)
        {
            _rmaServiceClient = rmaServiceClient;
            _filesContainer = filesContainer;
            _customerSuggestionProvider = customerSuggestionProvider;
            _responsiblePersonSuggestionProvider = responsiblePersonSuggestionProvider;
            _catalogUIOperations = catalogUIOperations;
            _catalogSelectionOperations = catalogSelectionOperations;
            _customerRepository = customerRepository;
            _responsiblePersonRepository = responsiblePersonRepository;
            _dictionaryRepository = dictionaryRepository;
        }

        public BranchType BranchType => BranchType.MillingCenter;

        public IWizardStepViewModel[] GetSteps()
        {
            return new IWizardStepViewModel[]
            {
                new MillingCenterInfoWizardStepViewModel(_catalogSelectionOperations, _catalogUIOperations, _customerSuggestionProvider, _responsiblePersonSuggestionProvider, _customerRepository, _responsiblePersonRepository),
                new MillingCenterProjectWizardStepViewModel(new ToothCardViewModel(_dictionaryRepository, new ClipboardHelper()), _filesContainer),
                new MillingCenterAdditionalEquipmentViewModel(_dictionaryRepository),
            };
        }
    }
}