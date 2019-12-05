using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Germadent.Rma.App.Printing;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.Views;
using Germadent.Rma.Model;
using Germadent.UI.Commands;
using Germadent.UI.Infrastructure;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private readonly IRmaOperations _rmaOperations;
        private readonly IWindowManager _windowManager;
        private readonly IShowDialogAgent _dialogAgent;
        private readonly IPrintModule _printModule;
        private OrderLightViewModel _selectedOrder;

        public MainViewModel(IRmaOperations rmaOperations, IWindowManager windowManager, IShowDialogAgent dialogAgent, IPrintModule printModule)
        {
            _rmaOperations = rmaOperations;
            _windowManager = windowManager;
            _dialogAgent = dialogAgent;
            _printModule = printModule;

            SelectedOrder = Orders.FirstOrDefault();

            CreateLabOrderCommand = new DelegateCommand(x => CreateLabOrderCommandHandler());
            CreateMillingCenterOrderCommand = new DelegateCommand(x => CreateMillingCenterOrderCommandHandler());
            FilterOrdersCommand = new DelegateCommand(x => FilterOrdersCommandHandler());
            CloseOrderCommand = new DelegateCommand(x => CloseOrderCommandHandler(), x => CanCloseOrderCommandHandler());
            PrintOrderCommand = new DelegateCommand(x => PrintOrderCommandHandler(), x => CanPrintOrderCommandHandler());
            OpenOrderCommand = new DelegateCommand(x => OpenOrderCommandHandler(), x => CanOpenOrderCommandHandler());

            FillOrders();
        }

        public ObservableCollection<OrderLightViewModel> Orders { get; } = new ObservableCollection<OrderLightViewModel>();

        public OrderLightViewModel SelectedOrder
        {
            get { return _selectedOrder; }
            set
            {
                if (_selectedOrder == value)
                    return;

                _selectedOrder = value;
                OnPropertyChanged(() => SelectedOrder);
            }
        }

        public IDelegateCommand CreateLabOrderCommand { get; }

        public IDelegateCommand CreateMillingCenterOrderCommand { get; }

        public IDelegateCommand FilterOrdersCommand { get; }

        public IDelegateCommand CloseOrderCommand { get; }

        public IDelegateCommand PrintOrderCommand { get; }

        public IDelegateCommand OpenOrderCommand { get; }

        private void CreateLabOrderCommandHandler()
        {
            var labOrder = _windowManager.CreateLabOrder(new LaboratoryOrder { Created = DateTime.Now });
            if (labOrder == null)
                return;

            var order = _rmaOperations.AddLaboratoryOrder(labOrder);
            Orders.Add(new OrderLightViewModel(order));
        }

        private void CreateMillingCenterOrderCommandHandler()
        {
            _windowManager.CreateMillingCenterOrder();
        }

        private void FilterOrdersCommandHandler()
        {
            var filter = _windowManager.CreateOrdersFilter();
            if (filter == null)
                return;

            FillOrders(filter);
        }

        private void FillOrders(OrdersFilter filter = null)
        {
            Orders.Clear();
            var orders = _rmaOperations.GetOrders(filter);
            foreach (var order in orders)
            {
                Orders.Add(new OrderLightViewModel(order));
            }
        }

        private bool CanCloseOrderCommandHandler()
        {
            return SelectedOrder != null;
        }

        private void CloseOrderCommandHandler()
        {
            if (_dialogAgent.ShowMessageDialog("Вы действительно хотите закрыть заказ наряд?", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            SelectedOrder.Model.Closed = DateTime.Now;
        }

        private bool CanPrintOrderCommandHandler()
        {
            return SelectedOrder != null;
        }

        private void PrintOrderCommandHandler()
        {
            _printModule.Print(SelectedOrder.Model);
        }

        private bool CanOpenOrderCommandHandler()
        {
            return SelectedOrder != null;
        }

        private void OpenOrderCommandHandler()
        {
            if (SelectedOrder.Model.BranchType == BranchType.Laboratory)
            {
                var labOrder = _rmaOperations.GetOrderDetails<LaboratoryOrder>(SelectedOrder.Model.Id);
                var changedLabOrder  = _windowManager.CreateLabOrder(labOrder);
                if (changedLabOrder == null)
                    return;

                var labOrderFromService = _rmaOperations.UpdateLaboratoryOrder(changedLabOrder);
                SelectedOrder.Update(labOrderFromService);
            }
        }
    }
}