using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Germadent.Rma.App.Operations;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;
using Germadent.Rma.App.Views;
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
        private readonly IPriceGroupRepository _priceGroupRepository;
        private readonly IPricePositionRepository _pricePositionRepository;
        private readonly IShowDialogAgent _dialogAgent;
        private readonly IRmaServiceClient _serviceClient;
        private readonly IAddPricePositionViewModel _addPricePositionViewModel;
        private PriceGroupViewModel _selectedGroup;
        private PricePositionViewModel _selectedPricePosition;

        private readonly ICollectionView _positionsView;

        public PriceListEditorViewModel(IUserManager userManager, 
            IPriceGroupRepository priceGroupRepository, 
            IPricePositionRepository pricePositionRepository, 
            IShowDialogAgent dialogAgent,
            IRmaServiceClient serviceClient,
            IAddPricePositionViewModel addPricePositionViewModel)
        {
            _priceGroupRepository = priceGroupRepository;
            _pricePositionRepository = pricePositionRepository;
            _dialogAgent = dialogAgent;
            _serviceClient = serviceClient;
            _addPricePositionViewModel = addPricePositionViewModel;

            Groups = new ObservableCollection<PriceGroupViewModel>();
            Positions = new ObservableCollection<PricePositionViewModel>();

            _positionsView = CollectionViewSource.GetDefaultView(Positions);
            _positionsView.Filter = PricePositionFilter;

            AddPriceGroupCommand = new DelegateCommand(AddPriceGroupCommandHandler, CanAddPriceGroupCommandHandler);
            EditPriceGroupCommand = new DelegateCommand(EditPriceGroupCommandHandler, CanEditPriceGroupCommandHandler);
            DeletePriceGroupCommand = new DelegateCommand(DeletePriceGroupCommandHandler, CanDeletePriceGroupCommandHandler);

            CanEditPriceList = userManager.HasRight(RmaUserRights.EditPriceList);
        }

        private bool PricePositionFilter(object obj)
        {
            if (SelectedGroup == null)
                return false;

            var pricePosition = (PricePositionViewModel)obj;
            return SelectedGroup.PriceGroupId == pricePosition.PriceGroupId;
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

            Positions.Clear();
            var positions = _pricePositionRepository.Items.Where(x => x.BranchType == BranchType).ToArray();
            foreach (var pricePositionDto in positions)
            {
                Positions.Add(new PricePositionViewModel(pricePositionDto));
            }
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

                _positionsView.Refresh();
            }
        }

        public ObservableCollection<PricePositionViewModel> Positions { get; }

        public PricePositionViewModel SelectedPosition
        {
            get { return _selectedPricePosition; }
            set
            {
                if (_selectedPricePosition == value)
                    return;
                _selectedPricePosition = value;
                OnPropertyChanged(() => SelectedPosition);
            }
        }

        public IDelegateCommand AddPriceGroupCommand { get; }

        public IDelegateCommand EditPriceGroupCommand { get; }

        public IDelegateCommand DeletePriceGroupCommand { get; }

        public IDelegateCommand AddPricePositionCommand { get; }

        public IDelegateCommand EditPricePositionCommand { get; }

        public IDelegateCommand DeletePricePositionCommand { get; }

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
            _serviceClient.UpdatePriceGroup(SelectedGroup.ToDto());
        }

        private bool CanDeletePriceGroupCommandHandler()
        {
            if (!CanEditPriceList)
                return false;

            if (SelectedGroup == null)
                return false;

            var positionsFromGroup = Positions.Where(x => x.PriceGroupId == SelectedGroup.PriceGroupId).ToArray();
            if (positionsFromGroup.Any())
                return false;

            return true;
        }

        private void DeletePriceGroupCommandHandler()
        {
            var message = "Вы действительно хотите удалить ценовую группу?";
            if (_dialogAgent.ShowMessageDialog(message, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;

            _serviceClient.DeletePriceGroup(SelectedGroup.PriceGroupId);

            Groups.Remove(SelectedGroup);
        }

        private bool CanAddPricePositionCommandHandler()
        {
            return CanEditPriceList;
        }
        private void AddPricePositionCommandHandler()
        {
            var allUserCodes = Positions.Select(x => x.UserCode).ToArray();
            _addPricePositionViewModel.Initialize(CardViewMode.Add, new PricePositionDto(), allUserCodes, BranchType);
            if (_dialogAgent.ShowDialog<AddPricePositionWindow>(_addPricePositionViewModel) == false)
                return;

            var pricePositionDto = _serviceClient.AddPricePosition(_addPricePositionViewModel.GetPricePosition());
            Positions.Add(new PricePositionViewModel(pricePositionDto));
        }
    }
}