using Germadent.Rma.App.ViewModels;
using Germadent.Rma.Model;
using Germadent.UI.Infrastructure;

namespace Germadent.Rma.App.Views
{
    public interface IWindowManager
    {
        Order CreateLabOrder();

        Order CreateMillingCenterOrder();

        OrdersFilter CreateOrdersFilter();
    }

    public class WindowManager : IWindowManager
    {
        private readonly IShowDialogAgent _dialogAgent;
        private readonly ILabOrderViewModel _labOrderViewModel;
        private readonly IMillingOrderViewModel _millingOrderViewModel;
        private readonly IOrdersFilterViewModel _ordersFilterViewModel;

        public WindowManager(IShowDialogAgent dialogAgent,
            ILabOrderViewModel labOrderViewModel,
            IMillingOrderViewModel millingOrderViewModel, 
            IOrdersFilterViewModel ordersFilterViewModel)
        {
            _dialogAgent = dialogAgent;
            _labOrderViewModel = labOrderViewModel;
            _millingOrderViewModel = millingOrderViewModel;
            _ordersFilterViewModel = ordersFilterViewModel;
        }

        public Order CreateLabOrder()
        {
            _labOrderViewModel.Initialize(false);
            if (_dialogAgent.ShowDialog<LabOrderWindow>(_labOrderViewModel) == true)
            {
                return new Order();
            }

            return null;
        }

        public Order CreateMillingCenterOrder()
        {
            _millingOrderViewModel.Initialize(false);
            if (_dialogAgent.ShowDialog<MillingCenterOrderWindow>(_millingOrderViewModel) == true)
            {
                return new Order();
            }

            return null;
        }

        public OrdersFilter CreateOrdersFilter()
        {
            if (_dialogAgent.ShowDialog<OrdersFilterWindow>(_ordersFilterViewModel) == true)
            {
                return _ordersFilterViewModel.GetFilter();
            }

            return null;
        }
    }
}