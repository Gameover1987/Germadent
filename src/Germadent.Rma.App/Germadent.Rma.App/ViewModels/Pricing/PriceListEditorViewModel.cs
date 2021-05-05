using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using Germadent.Client.Common.Infrastructure;
using Germadent.Client.Common.ServiceClient;
using Germadent.Common;
using Germadent.Common.Extensions;
using Germadent.Model;
using Germadent.Model.Pricing;
using Germadent.Model.Rights;
using Germadent.Rma.App.Infrastructure;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;
using Germadent.Rma.App.Views.Pricing;
using Germadent.UI.Commands;
using Germadent.UI.Infrastructure;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.Pricing
{
    public class PriceListEditorViewModel : ViewModelBase, IPriceListEditorViewModel
    {
        private readonly IPriceGroupRepository _priceGroupRepository;
        private readonly IPricePositionRepository _pricePositionRepository;
        private readonly IShowDialogAgent _dialogAgent;
        private readonly IRmaServiceClient _serviceClient;
        private readonly IAddPricePositionViewModel _addPricePositionViewModel;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ICommandExceptionHandler _commandExceptionHandler;
        private PriceGroupViewModel _selectedGroup;
        private PricePositionViewModel _selectedPricePosition;

        private readonly ICollectionView _positionsView;
        private bool _isBusy;
        private string _busyReason;

        public PriceListEditorViewModel(IUserManager userManager,
            IPriceGroupRepository priceGroupRepository,
            IPricePositionRepository pricePositionRepository,
            IShowDialogAgent dialogAgent,
            IRmaServiceClient serviceClient,
            IAddPricePositionViewModel addPricePositionViewModel,
            IDateTimeProvider dateTimeProvider, 
            ICommandExceptionHandler commandExceptionHandler)
        {
            _priceGroupRepository = priceGroupRepository;
            _pricePositionRepository = pricePositionRepository;
            _dialogAgent = dialogAgent;
            _serviceClient = serviceClient;
            _addPricePositionViewModel = addPricePositionViewModel;
            _dateTimeProvider = dateTimeProvider;
            _commandExceptionHandler = commandExceptionHandler;

            Groups = new ObservableCollection<PriceGroupViewModel>();
            Positions = new ObservableCollection<PricePositionViewModel>();

            _positionsView = CollectionViewSource.GetDefaultView(Positions);
            _positionsView.Filter = PricePositionFilter;

            AddPriceGroupCommand = new DelegateCommand(AddPriceGroupCommandHandler, CanAddPriceGroupCommandHandler);
            EditPriceGroupCommand = new DelegateCommand(EditPriceGroupCommandHandler, CanEditPriceGroupCommandHandler);
            DeletePriceGroupCommand = new DelegateCommand(DeletePriceGroupCommandHandler, CanDeletePriceGroupCommandHandler);

            AddPricePositionCommand = new DelegateCommand(AddPricePositionCommandHandler, CanAddPricePositionCommandHandler);
            EditPricePositionCommand = new DelegateCommand(EditPricePositionCommandHandler, CanEditPricePositionCommandHandler);
            DeletePricePositionCommand = new DelegateCommand(DeletePricePositionCommandHandler, CanDeletePricePositionCommandHandler);

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
            var groups = _priceGroupRepository.Items
                .Where(x => x.BranchType == BranchType)
                .OrderBy(x => x.Name).ToArray();
            foreach (var priceGroupDto in groups)
            {
                Groups.Add(new PriceGroupViewModel(priceGroupDto));
            }

            SelectedGroup = Groups.FirstOrDefault();

            Positions.Clear();
            var positions = _pricePositionRepository.Items.Where(x => x.BranchType == BranchType)
                .OrderBy(x => x.Name)
                .ToArray();
            foreach (var pricePositionDto in positions)
            {
                Positions.Add(new PricePositionViewModel(pricePositionDto, _dateTimeProvider));
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

        public bool IsBusy
        {
            get { return _isBusy; }
            private set
            {
                if (_isBusy == value)
                    return;

                _isBusy = value;
                OnPropertyChanged(() => IsBusy);

                IsBusyChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public string BusyReason
        {
            get { return _busyReason; }
            private set
            {
                if (_busyReason == value)
                    return;
                _busyReason = value;
                OnPropertyChanged(() => BusyReason);

                IsBusy = value != null;
            }
        }

        public event EventHandler<EventArgs> IsBusyChanged;

        public IDelegateCommand AddPriceGroupCommand { get; }

        public IDelegateCommand EditPriceGroupCommand { get; }

        public IDelegateCommand DeletePriceGroupCommand { get; }

        public IDelegateCommand AddPricePositionCommand { get; }

        public IDelegateCommand EditPricePositionCommand { get; }

        public IDelegateCommand DeletePricePositionCommand { get; }

        private bool CanAddPriceGroupCommandHandler()
        {
            return CanEditPriceList && !IsBusy;
        }

        private async void AddPriceGroupCommandHandler()
        {
            var priceGroupName = _dialogAgent.ShowInputBox("Добавление ценовой группы", "Ценовая группа");
            if (priceGroupName == null)
                return;

            var priceGroup = new PriceGroupDto { BranchType = BranchType, Name = priceGroupName };

            BusyReason = "Добавление ценовой группы";

            PriceGroupDto addedPriceGroup = null;
            try
            {
                await ThreadTaskExtensions.Run(() =>
                {
                    addedPriceGroup = _serviceClient.AddPriceGroup(priceGroup);
                });
                var priceGroupViewModel = new PriceGroupViewModel(addedPriceGroup);
                Groups.Add(priceGroupViewModel);
                SelectedGroup = priceGroupViewModel;
            }
            catch (Exception exception)
            {
                _commandExceptionHandler.HandleCommandException(exception);
            }
            finally
            {
                BusyReason = null;
            }
        }

        private bool CanEditPriceGroupCommandHandler()
        {
            return CanEditPriceList && SelectedGroup != null && !IsBusy;
        }

        private async void EditPriceGroupCommandHandler()
        {
            var priceGroupName = _dialogAgent.ShowInputBox("Изменение названия ценовой группы", "Ценовая группа", SelectedGroup.DisplayName);
            if (priceGroupName == null)
                return;

            SelectedGroup.DisplayName = priceGroupName;
            BusyReason = "Обновление ценовой группы";
            try
            {
                await ThreadTaskExtensions.Run(() =>
                {
                    _serviceClient.UpdatePriceGroup(SelectedGroup.ToDto());
                });
            }
            catch (Exception exception)
            {
                _commandExceptionHandler.HandleCommandException(exception);
            }
            finally
            {
                BusyReason = null;
            }
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

        private async void DeletePriceGroupCommandHandler()
        {
            var message = "Вы действительно хотите удалить ценовую группу?";
            if (_dialogAgent.ShowMessageDialog(message, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;

            try
            {
                BusyReason = "Удаление ценовой группы";
                await ThreadTaskExtensions.Run(() =>
                {
                    _serviceClient.DeletePriceGroup(SelectedGroup.PriceGroupId);
                });
                Groups.Remove(SelectedGroup);
            }
            catch (Exception exception)
            {
                _commandExceptionHandler.HandleCommandException(exception);
            }
            finally
            {
                BusyReason = null;
            }
        }

        private bool CanAddPricePositionCommandHandler()
        {
            return CanEditPriceList && SelectedGroup != null && !IsBusy;
        }
        private async void AddPricePositionCommandHandler()
        {
            var allUserCodes = Positions.Select(x => x.UserCode).ToArray();
            _addPricePositionViewModel.Initialize(CardViewMode.Add, new PricePositionDto { PriceGroupId = SelectedGroup.PriceGroupId }, allUserCodes, BranchType);
            if (_dialogAgent.ShowDialog<AddPricePositionWindow>(_addPricePositionViewModel) == false)
                return;

            BusyReason = "Добавление ценовой группы";
            PricePositionDto pricePositionDto = null;
            try
            {
                await ThreadTaskExtensions.Run(() =>
                {
                    pricePositionDto = _serviceClient.AddPricePosition(_addPricePositionViewModel.GetPricePosition());
                });
                var pricePositionViewModel = new PricePositionViewModel(pricePositionDto, _dateTimeProvider);
                Positions.Add(pricePositionViewModel);
                SelectedPosition = pricePositionViewModel;
            }
            catch (Exception exception)
            {
                _commandExceptionHandler.HandleCommandException(exception);
            }
            finally
            {
                BusyReason = null;
            }
        }

        private bool CanEditPricePositionCommandHandler()
        {
            return CanEditPriceList && SelectedPosition != null && SelectedGroup != null && !IsBusy;
        }

        private async void EditPricePositionCommandHandler()
        {
            var allUserCodes = Positions.Select(x => x.UserCode).ToArray();
            _addPricePositionViewModel.Initialize(CardViewMode.Edit, SelectedPosition.ToDto(), allUserCodes, BranchType);
            if (_dialogAgent.ShowDialog<AddPricePositionWindow>(_addPricePositionViewModel) == false)
                return;

            BusyReason = "Обновление ценовой позиции";
            PricePositionDto pricePositionDto = null;
            try
            {
                await ThreadTaskExtensions.Run(() =>
                {
                    pricePositionDto = _serviceClient.UpdatePricePosition(_addPricePositionViewModel.GetPricePosition());
                });
                SelectedPosition.Update(pricePositionDto);
            }
            catch (Exception exception)
            {
                _commandExceptionHandler.HandleCommandException(exception);
            }
            finally
            {
                BusyReason = null;
            }
        }

        private bool CanDeletePricePositionCommandHandler()
        {
            return CanEditPriceList && SelectedPosition != null;
        }

        private async void DeletePricePositionCommandHandler()
        {
            var message = "Вы действительно хотите удалить ценовую позицию?";
            if (_dialogAgent.ShowMessageDialog(message, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;

            BusyReason = "Удаление ценовой позиции";
            try
            {
                await ThreadTaskExtensions.Run(() =>
                {
                    _serviceClient.DeletePricePosition(SelectedPosition.PricePositionId);
                });
                Positions.Remove(SelectedPosition);
            }
            catch (Exception exception)
            {
                _commandExceptionHandler.HandleCommandException(exception);
            }
            finally
            {
                BusyReason = null;
            }
        }
    }
}