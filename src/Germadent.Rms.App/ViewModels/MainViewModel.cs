using Germadent.Common.Logging;
using Germadent.UI.Commands;
using Germadent.UI.Infrastructure;
using Germadent.UI.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using Germadent.Common.Extensions;
using System.Windows;
using Germadent.Client.Common.Infrastructure;
using Germadent.Client.Common.Reporting;
using Germadent.Client.Common.ServiceClient.Notifications;
using Germadent.Client.Common.ViewModels;
using Germadent.Model;
using Germadent.Model.Rights;
using Germadent.Rma.App.Views;
using Germadent.Rms.App.Properties;
using Germadent.Rms.App.ServiceClient;
using Germadent.Rms.App.Views;

namespace Germadent.Rms.App.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private readonly ILogger _logger;
        private readonly IRmsServiceClient _rmsServiceClient;
        private readonly IEnvironment _environment;
        private readonly IUserManager _userManager;
        private readonly IShowDialogAgent _dialogAgent;
        private readonly IOrdersFilterViewModel _ordersFilterViewModel;
        private readonly IStartWorkListViewModel _startWorkListViewModel;
        private readonly IFinishWorkListViewModel _finishWorkListViewModel;
        private readonly ISignalRClient _signalRClient;
        private readonly IPrintModule _printModule;
        private OrderLiteViewModel _selectedOrder;
        private bool _isBusy;
        private string _searchString;

        private readonly ListCollectionView _ordersView;
        private OrdersFilter _ordersFilter = OrdersFilter.CreateDefault();

        public MainViewModel(ILogger logger,
            IRmsServiceClient rmsServiceClient,
            IEnvironment environment,
            IUserManager userManager,
            IShowDialogAgent dialogAgent,
            IOrdersFilterViewModel ordersFilterViewModel,
            IStartWorkListViewModel startWorkListViewModel,
            IFinishWorkListViewModel finishWorkListViewModel,
            ISignalRClient signalRClient,
            IPrintModule printModule)
        {
            _logger = logger;
            _rmsServiceClient = rmsServiceClient;
            _environment = environment;
            _userManager = userManager;
            _dialogAgent = dialogAgent;
            _ordersFilterViewModel = ordersFilterViewModel;
            _startWorkListViewModel = startWorkListViewModel;
            _finishWorkListViewModel = finishWorkListViewModel;
            _signalRClient = signalRClient;
            _printModule = printModule;
            _signalRClient.WorkOrderLockedOrUnlocked += SignalRClientOnWorkOrderLockedOrUnlocked;
            _signalRClient.WorkOrderStatusChanged += SignalRClientOnWorkOrderStatusChanged;

            _ordersView = (ListCollectionView)CollectionViewSource.GetDefaultView(Orders);
            _ordersView.CustomSort = new OrderLiteComparerByDateTime();
            _ordersView.Filter = Filter;

            BeginWorkByWorkOrderCommand = new DelegateCommand(BeginWorksByWorkOrderCommandHandler, CanBeginWorksByWorkOrderCommandHandler);
            FinishWorkByWorkOrderCommand = new DelegateCommand(FinishWorkByWorkOrderCommandHandler, CanFinishWorksByWorkOrderCommandHandler);
            RealizeWorkOrderCommand = new DelegateCommand(PerfrormQualityControlCommandHandler, CanRealizeWorkOrderCommandHandler);

            FilterOrdersCommand = new DelegateCommand(x => FilterOrdersCommandHandler());
            PrintOrderCommand = new DelegateCommand(x => PrintOrderCommandHandler(), x => CanPrintOrderCommandHandler());
            LogOutCommand = new DelegateCommand(LogOutCommandHandler);
            ExitCommand = new DelegateCommand(ExitCommandHandler);

            CanQualityControl = _userManager.HasRight(RmsUserRights.QualityControl);
        }

        private bool Filter(object obj)
        {
            var order = (OrderLiteViewModel)obj;
            if (order.Status == OrderStatus.Closed)
                return false;

            if (SearchString.IsNullOrWhiteSpace())
                return true;

            return order.MatchBySearchString(SearchString);
        }

        public string Title => $"{Resources.AppTitle} - {_userManager.AuthorizationInfo.GetFullName()} ({_userManager.AuthorizationInfo.Login})";

        public ObservableCollection<OrderLiteViewModel> Orders { get; } = new ObservableCollection<OrderLiteViewModel>();

        public void Initialize()
        {
            FillOrders();
        }

        private void RefreshView()
        {
            _ordersView.Refresh();
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

        public bool CanQualityControl { get; }

        public IDelegateCommand BeginWorkByWorkOrderCommand { get; }

        public IDelegateCommand FinishWorkByWorkOrderCommand { get; }

        public IDelegateCommand RealizeWorkOrderCommand { get; }

        public IDelegateCommand FilterOrdersCommand { get; }

        public IDelegateCommand PrintOrderCommand { get; }

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

        private void FilterOrdersCommandHandler()
        {
            _ordersFilterViewModel.Initialize(_ordersFilter);
            if (_dialogAgent.ShowDialog<OrdersFilterWindow>(_ordersFilterViewModel) == false)
                return;

            _ordersFilter = _ordersFilterViewModel.GetFilter();
            FillOrders();
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
                    orders = _rmsServiceClient.GetOrders(_ordersFilter)
                        .Where(x => x.Status != OrderStatus.Closed)
                        .ToArray();
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

        private bool CanPrintOrderCommandHandler()
        {
            return SelectedOrder != null;
        }

        private void PrintOrderCommandHandler()
        {
            using (var orderScope = _rmsServiceClient.GetOrderById(SelectedOrder.WorkOrderId))
            {
                _printModule.PrintOrder(orderScope.Order);
            }
        }

        private bool CanBeginWorksByWorkOrderCommandHandler()
        {
            return SelectedOrder != null && (SelectedOrder.Status == OrderStatus.Created ||
                                             SelectedOrder.Status == OrderStatus.InProgress);
        }

        private void BeginWorksByWorkOrderCommandHandler()
        {
            using (var orderScope = _rmsServiceClient.GetOrderById(SelectedOrder.WorkOrderId))
            {
                _startWorkListViewModel.Initialize(orderScope.Order);
                if (_dialogAgent.ShowDialog<StartWorksWindow>(_startWorkListViewModel) == false)
                    return;

                var works = _startWorkListViewModel.GetWorks();
                _rmsServiceClient.StartWorks(works);
            }
        }

        private bool CanFinishWorksByWorkOrderCommandHandler()
        {
            return SelectedOrder != null && SelectedOrder.Status == OrderStatus.InProgress;
        }

        private void FinishWorkByWorkOrderCommandHandler()
        {
            using (var orderScope = _rmsServiceClient.GetOrderById(SelectedOrder.WorkOrderId))
            {
                _finishWorkListViewModel.Initialize(orderScope.Order);
                if (_dialogAgent.ShowDialog<FinishWorksWindow>(_finishWorkListViewModel) == false)
                    return;

                var works = _finishWorkListViewModel.GetWorks();
                _rmsServiceClient.FinishWorks(works);
            }
        }

        private bool CanRealizeWorkOrderCommandHandler()
        {
            if (!CanQualityControl)
                return false;

            return SelectedOrder != null && SelectedOrder.Status == OrderStatus.QualityControl;
        }

        private void PerfrormQualityControlCommandHandler()
        {
            var msg = $"Подтвердить прохождение контроля качества по заказ-наряду '{SelectedOrder.DocNumber}'?";
            if (_dialogAgent.ShowMessageDialog(msg, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _rmsServiceClient.PerformQualityControl(SelectedOrder.WorkOrderId);
            }
        }

        private void LogOutCommandHandler()
        {
            var msg = "Выйти из приложения, и зайти под другим пользователем?";
            if (_dialogAgent.ShowMessageDialog(msg, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _environment.Restart();
            }
        }

        private void ExitCommandHandler()
        {
            if (_dialogAgent.ShowMessageDialog("Выйти из приложения?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _environment.Shutdown();
            }
        }

        private void SignalRClientOnWorkOrderLockedOrUnlocked(object sender, OrderLockedEventArgs e)
        {
            var orderLiteViewModel = Orders.FirstOrDefault(x => x.WorkOrderId == e.Info.WorkOrderId);
            if (orderLiteViewModel == null)
                return;

            if (e.Info.IsLocked)
            {
                orderLiteViewModel.LockDate = e.Info.OccupancyDateTime;
                orderLiteViewModel.LockedBy = e.Info.User;
            }
            else
            {
                orderLiteViewModel.LockDate = null;
                orderLiteViewModel.LockedBy = null;
            }
        }

        private void SignalRClientOnWorkOrderStatusChanged(object sender, OrderStatusChangedEventArgs e)
        {
            var workOrder = Orders.FirstOrDefault(x => x.WorkOrderId == e.WorkOrderId);
            if (workOrder == null)
                return;

            workOrder.Status = e.Status;
            workOrder.StatusChanged = e.StatusChanged;

            _ordersView.Refresh();
        }
    }
}
