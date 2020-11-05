using System.Collections.ObjectModel;
using System.Linq;
using Germadent.Rma.App.Operations;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.Model;
using Germadent.UI.Commands;
using Germadent.UI.ViewModels;
using Germadent.UserManagementCenter.Model;
using Germadent.UserManagementCenter.Model.Rights;

namespace Germadent.Rma.App.ViewModels.Pricing
{
    public interface IPriceListEditorViewModel
    {
        BranchType BranchType { get; set; }

        void Initialize();
    }

    public class PriceListEditorViewModel : ViewModelBase, IPriceListEditorViewModel
    {
        private readonly IPriceListUIOperations _uiOperations;
        private readonly IUserManager _userManager;
        private readonly IPriceGroupRepository _priceGroupRepository;
        private readonly IPricePositionRepository _pricePositionRepository;
        private PriceGroupViewModel _selectedGroup;

        public PriceListEditorViewModel(IPriceListUIOperations uiOperations, IUserManager userManager, IPriceGroupRepository priceGroupRepository, IPricePositionRepository pricePositionRepository)
        {
            _uiOperations = uiOperations;
            _userManager = userManager;
            _priceGroupRepository = priceGroupRepository;
            _pricePositionRepository = pricePositionRepository;

            Groups = new ObservableCollection<PriceGroupViewModel>();
            Positions = new ObservableCollection<PricePositionViewModel>();

            AddPriceGroupCommand = new DelegateCommand(AddPriceGroupCommandHandler, CanAddPriceGroupCommandHandler);
            EditPriceGroupCommand = new DelegateCommand(EditPriceGroupCommandHandler, CanEditPriceGroupCommandHandler);
            DeletePriceGroupCommand = new DelegateCommand(DeletePriceGroupCommandHandler, CanDeletePriceGroupCommandHandler);

            CanEditPriceList = _userManager.HasRight(RmaUserRights.EditPriceList);
        }

        public bool CanEditPriceList { get; }

        public BranchType BranchType { get; set; }

        public void Initialize()
        {
            Groups.Clear();

            var groups = _priceGroupRepository.Items.Where(x => x.BranchType == BranchType).ToArray();
            foreach (var priceGroupDto in groups)
            {
                Groups.Add(new PriceGroupViewModel(priceGroupDto));
            }

            SelectedGroup = Groups.FirstOrDefault();
        }

        public ObservableCollection<PriceGroupViewModel> Groups { get; }

        public PriceGroupViewModel SelectedGroup
        {
            get { return _selectedGroup; }
            set
            {
                if (_selectedGroup == value)
                    return;
                _selectedGroup = value;
                OnPropertyChanged(() => SelectedGroup);
            }
        }

        public ObservableCollection<PricePositionViewModel> Positions { get; }

        public IDelegateCommand AddPriceGroupCommand { get; }

        public IDelegateCommand EditPriceGroupCommand { get; }

        public IDelegateCommand DeletePriceGroupCommand { get; }

        private bool CanAddPriceGroupCommandHandler()
        {
            return CanEditPriceList;
        }

        private void AddPriceGroupCommandHandler()
        {
            var priceGroup = _uiOperations.AddPriceGroup(BranchType);
            if (priceGroup == null)
                return;
        }

        private bool CanEditPriceGroupCommandHandler()
        {
            return CanEditPriceList;
        }

        private void EditPriceGroupCommandHandler()
        {

        }

        private bool CanDeletePriceGroupCommandHandler()
        {
            return CanEditPriceList;
        }

        private void DeletePriceGroupCommandHandler()
        {

        }
    }
}