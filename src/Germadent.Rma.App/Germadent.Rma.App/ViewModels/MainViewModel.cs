using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.Views;
using Germadent.Rma.Model;
using Germadent.UI.Commands;
using Germadent.UI.Infrastructure;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels
{
    public interface IMainViewModel
    { }

    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private readonly IRmaOperations _rmaOperations;
        private readonly IWindowManager _windowManager;
        private readonly IShowDialogAgent _dialogAgent;
        private Order _selectedOrder;

        public MainViewModel(IRmaOperations rmaOperations, IWindowManager windowManager, IShowDialogAgent dialogAgent)
        {
            _rmaOperations = rmaOperations;
            _windowManager = windowManager;
            _dialogAgent = dialogAgent;
            
            SelectedOrder = Orders.FirstOrDefault();

            CreateLabOrderCommand = new DelegateCommand(x => CreateLabOrderCommandHandler());
            CreateMillingCenterOrderCommand = new DelegateCommand(x => CreateMillingCenterOrderCommandHandler());
            FilterOrdersCommand = new DelegateCommand(x => FilterOrdersCommandHandler());
            CloseOrderCommand = new DelegateCommand(x => CloseOrderCommandHandler(), x => CanCloseOrderCommandHandler());

            FillOrders();
        }

        public ObservableCollection<Order> Orders { get; } = new ObservableCollection<Order>();

        public Order SelectedOrder
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

        public ICommand CreateLabOrderCommand { get; }

        public ICommand CreateMillingCenterOrderCommand { get; }

        public ICommand FilterOrdersCommand { get; }

        public ICommand CloseOrderCommand { get; }

        private void CreateLabOrderCommandHandler()
        {
             _windowManager.CreateLabOrder();
        }

        private void CreateMillingCenterOrderCommandHandler()
        {
            _windowManager.CreateMillingCenterOrder();
        }

        private void FilterOrdersCommandHandler()
        {
            var filter =_windowManager.CreateOrdersFilter();
            if (filter == null)
                return;

            FillOrders(filter);
        }

        private bool CanCloseOrderCommandHandler()
        {
            return SelectedOrder != null;
        }

        private void CloseOrderCommandHandler()
        {
            if (_dialogAgent.ShowMessageDialog("Вы действительно хотите закрыть заказ наряд?",MessageBoxButton.YesNo,MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            SelectedOrder.Closed = DateTime.Now;
        }

        private void FillOrders(OrdersFilter filter = null)
        {
            Orders.Clear();
            var orders = _rmaOperations.GetOrders(filter);
            foreach (var order in orders)
            {
                Orders.Add(order);
            }
        }
    }
}
