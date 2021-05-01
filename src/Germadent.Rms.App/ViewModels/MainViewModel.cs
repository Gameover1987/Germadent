using Germadent.Common.Logging;
using Germadent.Rma.App.Infrastructure;
using Germadent.Rma.Model;
using Germadent.Rma.App.Operations;
using Germadent.Rma.App.Properties;
using Germadent.Rma.App.ViewModels;
using Germadent.UI.Commands;
using Germadent.UI.Infrastructure;
using Germadent.UI.ViewModels;
using Germadent.UserManagementCenter.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Germadent.Common.Extensions;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.Views.Wizard;
using System.Windows;

namespace Germadent.Rms.App.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private readonly IRmaServiceClient _rmaOperations;
        private readonly IEnvironment _environment;
        private readonly ILogger _logger;
        private readonly IUserManager _userManager;
        private readonly IUserSettingsManager _userSettingsManager;
        private readonly IOrderUIOperations _orderUIOperations;
        private readonly IShowDialogAgent _dialogAgent;
        private OrderLiteViewModel _selectedOrder;
        private bool _isBusy;
        private string _searchString;

        private ListCollectionView _collectionView;
        private OrdersFilter _ordersFilter = OrdersFilter.CreateDefault();

        public MainViewModel(IRmaServiceClient rmaOperations,
            ILogger logger,
            IUserManager userManager,
            IUserSettingsManager userSettingsManager,
            IOrderUIOperations orderUIOperations,
            IShowDialogAgent dialogAgent)
        {
            _rmaOperations = rmaOperations;
            _logger = logger;
            _userManager = userManager;
            _userSettingsManager = userSettingsManager;
            _orderUIOperations = orderUIOperations;
            _dialogAgent = dialogAgent;

            SelectedOrder = Orders.FirstOrDefault();

            ChangeColumnsVisibilityCommand = new DelegateCommand(ChangeColumnsVisibilityCommandHandler);
            OpenOrderCommand = new DelegateCommand(x => OpenOrderCommandHandler(), x => CanOpenOrderCommandHandler());
            LogOutCommand = new DelegateCommand(LogOutCommandHandler);
            ExitCommand = new DelegateCommand(ExitCommandHandler);

        }

        public string Title
        {
            get
            {
                return $"{Resources.AppTitle} - {_userManager.AuthorizationInfo.GetFullName()} ({_userManager.AuthorizationInfo.Login})";
            }
        }

        public bool CanViewTechnologyOperations { get; } = true;

        public ObservableCollection<OrderLiteViewModel> Orders { get; } = new ObservableCollection<OrderLiteViewModel>();
        public ObservableCollection<ContextMenuItemViewModel> ColumnContextMenuItems { get; } = new ObservableCollection<ContextMenuItemViewModel>();

        public void Initialize()
        {
            FillOrders();

            ColumnContextMenuItems.Clear();
            foreach (var columnInfo in _userSettingsManager.Columns)
            {
                ColumnContextMenuItems.Add(new ContextMenuItemViewModel
                {
                    Command = ChangeColumnsVisibilityCommand,
                    Header = columnInfo.DisplayName,
                    Parameter = columnInfo,
                    IsChecked = columnInfo.IsVisible
                });
            }
        }

        private void RefreshView()
        {
            _collectionView.Refresh();
        }

        public OrderLiteViewModel SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                if (_selectedOrder == value)
                    return;

                _selectedOrder = value;
                OnPropertyChanged(() => SelectedOrder);
            }
        }

        public IUserSettingsManager SettingsManager
        {
            get { return _userSettingsManager; }
        }

        public event EventHandler ColumnSettingsChanged;

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy == value)
                    return;
                _isBusy = value;
                OnPropertyChanged(() => IsBusy);
            }
        }

        public IDelegateCommand OpenOrderCommand { get; }
        public IDelegateCommand ChangeColumnsVisibilityCommand { get; }
        public IDelegateCommand LogOutCommand { get; }
        public IDelegateCommand ExitCommand { get; }


        public string SearchString
        {
            get => _searchString;
            set
            {
                if (_searchString == value)
                    return;
                _searchString = value;
                OnPropertyChanged(() => SearchString);

                RefreshView();
            }
        }

        private async void FillOrders()
        {
            IsBusy = true;
            try
            {
                Orders.Clear();
                OrderLiteDto[] orders = null;
                await ThreadTaskExtensions.Run(() =>
                {
                    orders = _rmaOperations.GetOrders(_ordersFilter);
                });

                foreach (var order in orders)
                {
                    Orders.Add(new OrderLiteViewModel(order));
                }
            }
            catch (Exception e)
            {
                _dialogAgent.ShowErrorMessageDialog(e.Message, e.StackTrace);
                _logger.Error(e);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ChangeColumnsVisibilityCommandHandler(object sender)
        {
            var columnInfo = (ColumnInfo)sender;
            columnInfo.IsVisible = !columnInfo.IsVisible;

            ColumnSettingsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void FilterOrdersCommandHandler()
        {
            var filter = _orderUIOperations.CreateOrdersFilter(_ordersFilter);
            if (filter == null)
                return;

            _ordersFilter = filter;
            FillOrders();
        }

        private void OpenOrderCommandHandler()
        {
            OrderDto changedOrderDto = null;
            var orderLiteViewModel = SelectedOrder;
            var orderDto = _rmaOperations.GetOrderById(orderLiteViewModel.Model.WorkOrderId);
            var wizardMode = orderDto.Closed == null ? WizardMode.Edit : WizardMode.View;
            if (orderDto.BranchType == BranchType.Laboratory)
            {
                changedOrderDto = _orderUIOperations.CreateLabOrder(orderDto, wizardMode);
            }
            else if (orderDto.BranchType == BranchType.MillingCenter)
            {
                changedOrderDto = _orderUIOperations.CreateMillingCenterOrder(orderDto, wizardMode);
            }

            if (changedOrderDto == null)
                return;

            orderLiteViewModel.Update(changedOrderDto.ToOrderLite());
        }

        private void LogOutCommandHandler()
        {
            if (_dialogAgent.ShowMessageDialog("Выйти из приложения, и зайти под другим пользователем?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;

            _environment.Restart();
        }

        private void ExitCommandHandler()
        {
            if (_dialogAgent.ShowMessageDialog("Выйти из приложения?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;

            _environment.Shutdown();
        }

        private bool CanOpenOrderCommandHandler()
        {
            return SelectedOrder != null;
        }
    }
}
