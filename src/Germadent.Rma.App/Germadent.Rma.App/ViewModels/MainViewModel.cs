using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Germadent.Rma.App.Views;
using Germadent.ServiceClient.Model;
using Germadent.ServiceClient.Operation;
using Germadent.UI.Commands;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels
{
    public interface IMainViewModel
    { }

    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private readonly IRmaOperations _rmaOperations;
        private readonly IWindowManager _windowManager;
        private Order _selectedOrder;

        public MainViewModel(IRmaOperations rmaOperations, IWindowManager windowManager)
        {
            _rmaOperations = rmaOperations;
            _windowManager = windowManager;

            Orders = new ObservableCollection<Order>(_rmaOperations.GetOrders());
            SelectedOrder = Orders.FirstOrDefault();

            CreateLabOrderCommand = new DelegateCommand(x => CreateLabOrderCommandHandler());
            CreateMillingCenterOrderCommand = new DelegateCommand(x => CreateMillingCenterOrderCommandHandler());
            FilterOrdersCommand = new DelegateCommand(x => FilterOrdersCommandHandler());
        }

        public ObservableCollection<Order> Orders { get; }

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

        private void CreateLabOrderCommandHandler()
        {
            var order = _windowManager.CreateLabOrder();
        }

        private void CreateMillingCenterOrderCommandHandler()
        {
            var order = _windowManager.CreateMillingCenterOrder();
        }

        private void FilterOrdersCommandHandler()
        {
            var filter = _windowManager.CreateOrdersFilter();
        }
    }
}
