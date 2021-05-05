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
using Germadent.Client.Common.ViewModels;
using Germadent.Model;
using Germadent.Rms.App.Properties;
using Germadent.Rms.App.ServiceClient;

namespace Germadent.Rms.App.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private readonly ILogger _logger;
        private readonly IRmsServiceClient _rmsServiceClient;
        private readonly IEnvironment _environment;
        private readonly IUserManager _userManager;
        private readonly IShowDialogAgent _dialogAgent;
        private OrderLiteViewModel _selectedOrder;
        private bool _isBusy;
        private string _searchString;

        private ListCollectionView _collectionView;
        private readonly OrdersFilter _ordersFilter = OrdersFilter.CreateDefault();

        public MainViewModel(ILogger logger,
            IRmsServiceClient rmsServiceClient,
            IEnvironment environment,
            IUserManager userManager,
            IShowDialogAgent dialogAgent)
        {
            _logger = logger;
            _rmsServiceClient = rmsServiceClient;
            _environment = environment;
            _userManager = userManager;
            _dialogAgent = dialogAgent;

            SelectedOrder = Orders.FirstOrDefault();
            
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

        public void Initialize()
        {
            FillOrders();
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
                    orders = _rmsServiceClient.GetOrders(_ordersFilter);
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

        private void FilterOrdersCommandHandler()
        {
            //var filter = _orderUIOperations.CreateOrdersFilter(_ordersFilter);
            //if (filter == null)
            //    return;

            //_ordersFilter = filter;
            //FillOrders();
        }

        private void OpenOrderCommandHandler()
        {
            OrderDto changedOrderDto = null;
            var orderLiteViewModel = SelectedOrder;
            var orderDto = _rmsServiceClient.GetOrderById(orderLiteViewModel.Model.WorkOrderId);
            //var wizardMode = orderDto.Closed == null ? WizardMode.Edit : WizardMode.View;
            //if (orderDto.BranchType == BranchType.Laboratory)
            //{
            //    changedOrderDto = _orderUIOperations.CreateLabOrder(orderDto, wizardMode);
            //}
            //else if (orderDto.BranchType == BranchType.MillingCenter)
            //{
            //    changedOrderDto = _orderUIOperations.CreateMillingCenterOrder(orderDto, wizardMode);
            //}

            //if (changedOrderDto == null)
            //    return;

            //orderLiteViewModel.Update(changedOrderDto.ToOrderLite());
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
