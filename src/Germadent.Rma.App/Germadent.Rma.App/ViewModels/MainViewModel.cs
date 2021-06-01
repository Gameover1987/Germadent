using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using Germadent.Client.Common.Infrastructure;
using Germadent.Client.Common.Reporting;
using Germadent.Client.Common.ServiceClient.Notifications;
using Germadent.Client.Common.ViewModels;
using Germadent.Common.Extensions;
using Germadent.Common.Logging;
using Germadent.Model;
using Germadent.Model.Rights;
using Germadent.Rma.App.Infrastructure;
using Germadent.Rma.App.Operations;
using Germadent.Rma.App.Properties;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ViewModels.Pricing;
using Germadent.Rma.App.ViewModels.Salary;
using Germadent.Rma.App.ViewModels.TechnologyOperation;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;
using Germadent.Rma.App.Views;
using Germadent.Rma.App.Views.Pricing;
using Germadent.Rma.App.Views.TechnologyOperation;
using Germadent.Rma.App.Views.Wizard;
using Germadent.UI.Commands;
using Germadent.UI.Infrastructure;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private readonly IRmaServiceClient _rmaServiceClient;
        private readonly IEnvironment _environment;
        private readonly IOrderUIOperations _orderUIOperations;
        private readonly IShowDialogAgent _dialogAgent;
        private readonly ICustomerCatalogViewModel _customerCatalogViewModel;
        private readonly IResponsiblePersonCatalogViewModel _responsiblePersonCatalogViewModel;
        private readonly IPriceListEditorContainerViewModel _priceListEditorContainerViewModel;
        private readonly ITechnologyOperationsEditorViewModel _technologyOperationsEditorViewModel;
        private readonly IPrintModule _printModule;
        private readonly ILogger _logger;
        private readonly IUserManager _userManager;
        private readonly IUserSettingsManager _userSettingsManager;
        private readonly IClipboardHelper _clipboardHelper;
        private readonly ISignalRClient _signalRClient;
        private readonly ISalaryCalculationViewModel _salaryCalculationViewModel;
        private OrderLiteViewModel _selectedOrder;
        private bool _isBusy;
        private string _searchString;

        private readonly ListCollectionView _collectionView;

        private OrdersFilter _ordersFilter = OrdersFilter.CreateDefault();

        public MainViewModel(IRmaServiceClient rmaServiceClient,
            IEnvironment environment,
            IOrderUIOperations orderUIOperations,
            IShowDialogAgent dialogAgent,
            ICustomerCatalogViewModel customerCatalogViewModel,
            IResponsiblePersonCatalogViewModel responsiblePersonCatalogViewModel,
            IPriceListEditorContainerViewModel priceListEditorContainerContainerViewModel,
            ITechnologyOperationsEditorViewModel technologyOperationsEditorViewModel,
            IPrintModule printModule,
            ILogger logger,
            IUserManager userManager,
            IUserSettingsManager userSettingsManager,
            IClipboardHelper clipboardHelper,
            ISignalRClient signalRClient,
            ISalaryCalculationViewModel salaryCalculationViewModel)
        {
            _rmaServiceClient = rmaServiceClient;
            _environment = environment;
            _orderUIOperations = orderUIOperations;
            _dialogAgent = dialogAgent;
            _customerCatalogViewModel = customerCatalogViewModel;
            _responsiblePersonCatalogViewModel = responsiblePersonCatalogViewModel;
            _priceListEditorContainerViewModel = priceListEditorContainerContainerViewModel;
            _technologyOperationsEditorViewModel = technologyOperationsEditorViewModel;
            _printModule = printModule;
            _logger = logger;
            _userManager = userManager;
            _userSettingsManager = userSettingsManager;
            _clipboardHelper = clipboardHelper;
            _signalRClient = signalRClient;
            _salaryCalculationViewModel = salaryCalculationViewModel;
            _signalRClient.WorkOrderLockedOrUnlocked += SignalRClientOnWorkOrderLockedOrUnlocked;
            _signalRClient.WorkOrderStatusChanged += SignalRClientOnWorkOrderStatusChanged;

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
            ShowPriceListEditorCommand = new DelegateCommand(ShowPriceListEditorCommandHandler, CanShowPriceListEditorCommandHandler);
            ShowTechnologyOperationsEditorCommand = new DelegateCommand(ShowTechnologyOperationsEditorCommandHandler, CanShowTechnologyOperationsEditorCommandHandler);
            ShowSalaryCalculationCommand = new DelegateCommand(ShowSalaryCalculationCommandHandler, CanShowSalaryCalculationCommandHandler);
            LogOutCommand = new DelegateCommand(LogOutCommandHandler);
            ExitCommand = new DelegateCommand(ExitCommandHandler);
            ChangeColumnsVisibilityCommand = new DelegateCommand(ChangeColumnsVisibilityCommandHandler);

            _collectionView = (ListCollectionView)CollectionViewSource.GetDefaultView(Orders);
            _collectionView.CustomSort = new OrderLiteComparerByDateTime();
            _collectionView.Filter = Filter;

            CanViewPriceList = _userManager.HasRight(RmaUserRights.ViewPriceList);
            CanCalculateSalary = _userManager.HasRight(RmaUserRights.SalaryCalculation);
        }

        public string Title
        {
            get
            {
                return $"{Resources.AppTitle} - {_userManager.AuthorizationInfo.GetFullName()} ({_userManager.AuthorizationInfo.Login})";
            }
        }

        public bool CanViewPriceList { get; }

        public bool CanViewTechnologyOperations { get; } = true;

        public bool CanCalculateSalary { get; } = true;

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

        public IDelegateCommand CreateLabOrderCommand { get; }

        public IDelegateCommand CreateMillingCenterOrderCommand { get; }

        public IDelegateCommand FilterOrdersCommand { get; }

        public IDelegateCommand CloseOrderCommand { get; }

        public IDelegateCommand PrintOrderCommand { get; }

        public IDelegateCommand OpenOrderCommand { get; }

        public IDelegateCommand CopyOrderToClipboardCommand { get; }

        public IDelegateCommand ShowCustomersDictionaryCommand { get; }

        public IDelegateCommand ShowResponsiblePersonsDictionaryCommand { get; }

        public IDelegateCommand ShowPriceListEditorCommand { get; }

        public IDelegateCommand ShowTechnologyOperationsEditorCommand { get; }

        public IDelegateCommand ShowSalaryCalculationCommand { get; }

        public IDelegateCommand LogOutCommand { get; }

        public IDelegateCommand ExitCommand { get; }

        public IDelegateCommand ChangeColumnsVisibilityCommand { get; }

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

        public ObservableCollection<ContextMenuItemViewModel> ColumnContextMenuItems { get; } = new ObservableCollection<ContextMenuItemViewModel>();

        private bool Filter(object obj)
        {
            if (SearchString.IsNullOrWhiteSpace())
                return true;

            var order = (OrderLiteViewModel)obj;
            return order.MatchBySearchString(SearchString);
        }

        private void RefreshView()
        {
            _collectionView.Refresh();
        }

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

        private void SignalRClientOnWorkOrderStatusChanged(object? sender, OrderStatusChangedEventArgs e)
        {
            var workOrder = Orders.FirstOrDefault(x => x.WorkOrderId == e.WorkOrderId);
            if (workOrder == null)
                return;

            workOrder.Status = e.Status;
            workOrder.StatusChanged = e.StatusChanged;
        }

        private void ChangeColumnsVisibilityCommandHandler(object sender)
        {
            var columnInfo = (ColumnInfo)sender;
            columnInfo.IsVisible = !columnInfo.IsVisible;

            ColumnSettingsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void CreateLabOrderCommandHandler()
        {
            var labOrder = _orderUIOperations.CreateLabOrder(new OrderDto
            {
                BranchType = BranchType.Laboratory,
                CreatorFullName = _rmaServiceClient.AuthorizationInfo.GetShortFullName()
            }, WizardMode.Create);

            if (labOrder == null)
                return;

            var orderLiteViewModel = new OrderLiteViewModel(labOrder.ToOrderLite());
            Orders.Add(orderLiteViewModel);
            SelectedOrder = orderLiteViewModel;
        }

        private void CreateMillingCenterOrderCommandHandler()
        {
            var millingCenterOrder = _orderUIOperations.CreateMillingCenterOrder(new OrderDto
            {
                BranchType = BranchType.MillingCenter,
                CreatorFullName = _rmaServiceClient.AuthorizationInfo.GetShortFullName()
            }, WizardMode.Create);

            if (millingCenterOrder == null)
            {
                return;
            }

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
                    orders = _rmaServiceClient.GetOrders(_ordersFilter);
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
            return SelectedOrder != null && SelectedOrder.Status == OrderStatus.Realization;
        }

        private void CloseOrderCommandHandler()
        {
            var message = "После закрытия заказ-наряда изменить его будет невозможно, только открыть или распечатать.\nВы действительно хотите закрыть заказ наряд?";
            if (_dialogAgent.ShowMessageDialog(message, MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;
            _rmaServiceClient.CloseOrder(SelectedOrder.WorkOrderId);
            SelectedOrder.Status = OrderStatus.Closed;
        }

        private bool CanPrintOrderCommandHandler()
        {
            return SelectedOrder != null;
        }

        private void PrintOrderCommandHandler()
        {
            using (var orderScope = _rmaServiceClient.GetOrderById(SelectedOrder.WorkOrderId))
            {
                _printModule.Print(orderScope.Order);
            }
        }

        private bool CanOpenOrderCommandHandler()
        {
            if (SelectedOrder == null)
                return false;

            if (SelectedOrder.IsLocked && SelectedOrder.LockedBy.UserId != _rmaServiceClient.AuthorizationInfo.UserId)
            {
                return false;
            }

            return true;
        }

        private void OpenOrderCommandHandler()
        {
            OrderDto changedOrderDto = null;
            var orderLiteViewModel = SelectedOrder;
            using (var orderScope = _rmaServiceClient.GetOrderById(orderLiteViewModel.WorkOrderId))
            {
                var orderDto = orderScope.Order;
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
        }

        private void CopyOrderToClipboardCommandHandler()
        {
            var reports = _rmaServiceClient.GetWorkReport(SelectedOrder.WorkOrderId);
            if (reports.Length == 0)
                return;

            var builder = new StringBuilder();

            foreach (var report in reports)
            {
                var line = string.Concat(report.Created == DateTime.MinValue ? string.Empty : report.Created.ToString(), "\t", report.DocNumber, "\t", report.Customer, "\t", report.EquipmentSubstring, "\t", report.Patient, "\t", report.ProstheticSubstring, "\t", report.MaterialsStr, "\t", report.ConstructionColor, "\t", report.Quantity, "\t", "\t", "\t", "\t", "\t", report.ImplantSystem, "\t", report.TotalPriceCashless, "\t", report.TotalPrice, "\t", report.ResponsiblePerson, "\n").Trim();
                builder.AppendLine(line);
            }

            var data = builder.ToString();

            _clipboardHelper.CopyToClipboard(data);
        }

        private void ShowCustomersDictionaryCommandHandler()
        {
            _dialogAgent.ShowDialog<CustomerCatalogWindow>(_customerCatalogViewModel);
        }

        private void ShowResponsiblePersonsDictionaryCommandHandler()
        {
            _dialogAgent.ShowDialog<ResponsiblePersonCatalogWindow>(_responsiblePersonCatalogViewModel);
        }

        private bool CanShowPriceListEditorCommandHandler()
        {
            return CanViewPriceList;
        }

        private void ShowPriceListEditorCommandHandler()
        {
            _dialogAgent.ShowDialog<PriceListEditorWindow>(_priceListEditorContainerViewModel);
        }

        private bool CanShowTechnologyOperationsEditorCommandHandler()
        {
            return CanViewTechnologyOperations;
        }

        private void ShowTechnologyOperationsEditorCommandHandler()
        {
            _dialogAgent.ShowDialog<TechnologyOperationsEditorWindow>(_technologyOperationsEditorViewModel);
        }

        private bool CanShowSalaryCalculationCommandHandler()
        {
            return CanCalculateSalary;
        }

        private void ShowSalaryCalculationCommandHandler()
        {
            _salaryCalculationViewModel.Initialize();
            _dialogAgent.ShowDialog<SalaryCalculationWindow>(_salaryCalculationViewModel);
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
    }
}