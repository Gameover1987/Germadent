using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Germadent.Common.Extensions;
using Germadent.Common.Logging;
using Germadent.Rma.App.Operations;
using Germadent.Rma.App.Reporting;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;
using Germadent.Rma.App.Views;
using Germadent.Rma.App.Views.Wizard;
using Germadent.Rma.Model;
using Germadent.UI.Commands;
using Germadent.UI.Infrastructure;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private readonly IRmaServiceClient _rmaOperations;
        private readonly IOrderUIOperations _orderUIOperations;
        private readonly IShowDialogAgent _dialogAgent;
        private readonly ICustomerCatalogViewModel _customerCatalogViewModel;
        private readonly IResponsiblePersonCatalogViewModel _responsiblePersonCatalogViewModel;
        private readonly IPrintModule _printModule;
        private readonly ILogger _logger;
        private readonly IReporter _reporter;
        private OrderLiteViewModel _selectedOrder;
        private bool _isBusy;
        private string _searchString;

        private readonly ICollectionView _collectionView;

        private OrdersFilter _ordersFilter = OrdersFilter.CreateDefault();

        public MainViewModel(IRmaServiceClient rmaOperations,
            IOrderUIOperations orderUIOperations,
            IShowDialogAgent dialogAgent,
            ICustomerCatalogViewModel customerCatalogViewModel,
            IResponsiblePersonCatalogViewModel responsiblePersonCatalogViewModel,
            IPrintModule printModule,
            ILogger logger,
            IReporter reporter)
        {
            _rmaOperations = rmaOperations;
            _orderUIOperations = orderUIOperations;
            _dialogAgent = dialogAgent;
            _customerCatalogViewModel = customerCatalogViewModel;
            _responsiblePersonCatalogViewModel = responsiblePersonCatalogViewModel;
            _printModule = printModule;
            _logger = logger;
            _reporter = reporter;

            SelectedOrder = Orders.FirstOrDefault();

            CreateLabOrderCommand = new DelegateCommand(x => CreateLabOrderCommandHandler());
            CreateMillingCenterOrderCommand = new DelegateCommand(x => CreateMillingCenterOrderCommandHandler());
            FilterOrdersCommand = new DelegateCommand(x => FilterOrdersCommandHandler());
            CloseOrderCommand = new DelegateCommand(x => CloseOrderCommandHandler(), x => CanCloseOrderCommandHandler());
            PrintOrderCommand = new DelegateCommand(x => PrintOrderCommandHandler(), x => CanPrintOrderCommandHandler());
            OpenOrderCommand = new DelegateCommand(x => OpenOrderCommandHandler(), x => CanOpenOrderCommandHandler());
            CopyOrderToClipboardCommand = new DelegateCommand(x => CopyOrderToClipboardCommandHandler());
            ShowCustomersDictionaryCommand = new DelegateCommand(ShowCustomersDictionaryCommandHandler);
            ShowResponsiblePersonsDictionaryCommand = new DelegateCommand(ShowResponsiblePersonsDictionaryCommandHandler);

            _collectionView = CollectionViewSource.GetDefaultView(Orders);
            _collectionView.Filter = Filter;
        }

        private bool Filter(object obj)
        {
            if (SearchString.IsNullOrWhiteSpace())
                return true;

            var order = (OrderLiteViewModel)obj;
            return order.MatchBySearchString(SearchString);
        }

        public ObservableCollection<OrderLiteViewModel> Orders { get; } = new ObservableCollection<OrderLiteViewModel>();

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

        public IDelegateCommand CreateLabOrderCommand { get; }

        public IDelegateCommand CreateMillingCenterOrderCommand { get; }

        public IDelegateCommand FilterOrdersCommand { get; }

        public IDelegateCommand CloseOrderCommand { get; }

        public IDelegateCommand PrintOrderCommand { get; }

        public IDelegateCommand OpenOrderCommand { get; }

        public IDelegateCommand CopyOrderToClipboardCommand { get; }

        public IDelegateCommand ShowCustomersDictionaryCommand { get; }

        public IDelegateCommand ShowResponsiblePersonsDictionaryCommand { get; }

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

        private void RefreshView()
        {
            _collectionView.Refresh();
        }

        public void Initialize()
        {
            FillOrders();
        }

        private void CreateLabOrderCommandHandler()
        {
            var labOrder = _orderUIOperations.CreateLabOrder(new OrderDto { BranchType = BranchType.Laboratory }, WizardMode.Create);

            if (labOrder == null)
                return;

            var orderLiteViewModel = new OrderLiteViewModel(labOrder.ToOrderLite());
            Orders.Add(orderLiteViewModel);
            SelectedOrder = orderLiteViewModel;
        }

        private void CreateMillingCenterOrderCommandHandler()
        {
            var millingCenterOrder = _orderUIOperations.CreateMillingCenterOrder(new OrderDto { BranchType = BranchType.MillingCenter }, WizardMode.Create);

            if (millingCenterOrder == null)
                return;

            var orderLiteViewModel = new OrderLiteViewModel(millingCenterOrder.ToOrderLite());
            Orders.Add(orderLiteViewModel);
            SelectedOrder = orderLiteViewModel;
        }

        private void FilterOrdersCommandHandler()
        {
            var filter = _orderUIOperations.CreateOrdersFilter(_ordersFilter);
            if (filter == null)
                return;

            _ordersFilter = filter;
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

        private bool CanCloseOrderCommandHandler()
        {
            return SelectedOrder != null && SelectedOrder.Model.Closed == null;
        }

        private void CloseOrderCommandHandler()
        {
            if (_dialogAgent.ShowMessageDialog("После закрытия заказ-наряда изменить его будет невозможно, только открыть или распечатать.\nВы действительно хотите закрыть заказ наряд?", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;
            var order = _rmaOperations.CloseOrder(SelectedOrder.Model.WorkOrderId);
            SelectedOrder.Update(order.ToOrderLite());
        }

        private bool CanPrintOrderCommandHandler()
        {
            return SelectedOrder != null;
        }

        private void PrintOrderCommandHandler()
        {
            _printModule.Print(_rmaOperations.GetOrderById(SelectedOrder.Model.WorkOrderId));
        }

        private bool CanOpenOrderCommandHandler()
        {
            return SelectedOrder != null;
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

        private void CopyOrderToClipboardCommandHandler()
        {
            var orderId = SelectedOrder.Model.WorkOrderId;
            _reporter.CreateReport(orderId);
        }

        private void ShowCustomersDictionaryCommandHandler()
        {
            _dialogAgent.ShowDialog<CustomerCatalogWindow>(_customerCatalogViewModel);
        }

        private void ShowResponsiblePersonsDictionaryCommandHandler()
        {
            _dialogAgent.ShowDialog<ResponsiblePersonCatalogWindow>(_responsiblePersonCatalogViewModel);
        }
    }
}