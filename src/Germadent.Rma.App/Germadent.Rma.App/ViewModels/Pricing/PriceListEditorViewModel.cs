using System.Collections.ObjectModel;
using System.Linq;
using Germadent.Rma.App.Operations;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.Model;
using Germadent.Rma.Model.Pricing;
using Germadent.UI.Commands;
using Germadent.UI.Infrastructure;
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
        private readonly IUserManager _userManager;
        private readonly IPriceGroupRepository _priceGroupRepository;
        private readonly IPricePositionRepository _pricePositionRepository;
        private readonly IShowDialogAgent _dialogAgent;
        private readonly IRmaServiceClient _serviceClient;
        private PriceGroupViewModel _selectedGroup;

        public PriceListEditorViewModel(IUserManager userManager, IPriceGroupRepository priceGroupRepository, IPricePositionRepository pricePositionRepository, IShowDialogAgent dialogAgent, IRmaServiceClient serviceClient)
        {
            _userManager = userManager;
            _priceGroupRepository = priceGroupRepository;
            _pricePositionRepository = pricePositionRepository;
            _dialogAgent = dialogAgent;
            _serviceClient = serviceClient;

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
            var priceGroupName = _dialogAgent.ShowInputBox("Добавление ценовой группы", "Ценовая группа");
            if (priceGroupName == null)
                return;

            var priceGroup = new PriceGroupDto { BranchType = BranchType, Name = priceGroupName };

            _serviceClient.AddPriceGroup(priceGroup);

            Groups.Add(new PriceGroupViewModel(priceGroup));
        }

        private bool CanEditPriceGroupCommandHandler()
        {
            return CanEditPriceList && SelectedGroup != null;
        }

        private void EditPriceGroupCommandHandler()
        {
            var priceGroupName = _dialogAgent.ShowInputBox("Изменение названия ценовой группы", "Ценовая группа", SelectedGroup.DisplayName);
            if (priceGroupName == null)
                return;

            SelectedGroup.DisplayName = priceGroupName;
            _serviceClient.AddPriceGroup(SelectedGroup.ToDto());
        }

        private bool CanDeletePriceGroupCommandHandler()
        {
            return CanEditPriceList && SelectedGroup != null;
        }

        private void DeletePriceGroupCommandHandler()
        {

        }
    }
}