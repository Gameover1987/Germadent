using Germadent.Rma.App.Operations;
using Germadent.Rma.App.Reporting;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.App.ViewModels.Pricing;
using Germadent.Rma.App.ViewModels.ToothCard;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public interface ILabWizardStepsProvider : IWizardStepsProvider
    {

    }

    public class LabWizardStepsProvider : ILabWizardStepsProvider
    {
        private readonly ICustomerSuggestionProvider _customerSuggestionProvider;
        private readonly IResponsiblePersonsSuggestionsProvider _responsiblePersonSuggestionProvider;
        private readonly ICatalogUIOperations _catalogUIOperations;
        private readonly ICatalogSelectionUIOperations _catalogSelectionOperations;
        private readonly ICustomerRepository _customerRepository;
        private readonly IResponsiblePersonRepository _responsiblePersonRepository;
        private readonly IDictionaryRepository _dictionaryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPriceListViewModel _priceListViewModel;
        private readonly IClipboardHelper _clipboardHelper;


        public LabWizardStepsProvider(ICustomerSuggestionProvider customerSuggestionProvider,
            IResponsiblePersonsSuggestionsProvider responsiblePersonSuggestionProvider,
            ICatalogUIOperations catalogUIOperations,
            ICatalogSelectionUIOperations catalogSelectionOperations,
            ICustomerRepository customerRepository,
            IResponsiblePersonRepository responsiblePersonRepository,
            IDictionaryRepository dictionaryRepository,
            IProductRepository productRepository,
            IPriceListViewModel priceListViewModel,
            IClipboardHelper clipboardHelper)
        {
            _customerSuggestionProvider = customerSuggestionProvider;
            _responsiblePersonSuggestionProvider = responsiblePersonSuggestionProvider;
            _catalogUIOperations = catalogUIOperations;
            _catalogSelectionOperations = catalogSelectionOperations;
            _customerRepository = customerRepository;
            _responsiblePersonRepository = responsiblePersonRepository;
            _dictionaryRepository = dictionaryRepository;
            _productRepository = productRepository;
            _priceListViewModel = priceListViewModel;
            _clipboardHelper = clipboardHelper;
        }

        public BranchType BranchType => BranchType.Laboratory;

        public IWizardStepViewModel[] GetSteps()
        {
            return new IWizardStepViewModel[]
            {
                new LaboratoryInfoWizardStepViewModel(_catalogSelectionOperations, _catalogUIOperations, _customerSuggestionProvider, _responsiblePersonSuggestionProvider, _customerRepository, _responsiblePersonRepository),
                new PriceListWizardStepViewModel(new ToothCardViewModel(_dictionaryRepository, _productRepository, _clipboardHelper), _priceListViewModel), 
            };
        }
    }
}