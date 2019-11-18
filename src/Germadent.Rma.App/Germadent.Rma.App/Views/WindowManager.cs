using Germadent.Rma.App.ViewModels;
using Germadent.Rma.App.ViewModels.Wizard;
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
        private readonly IWizardStepsProvider _labWizardProvider;
        
        private readonly IMillingOrderViewModel _millingOrderViewModel;
        private readonly IOrdersFilterViewModel _ordersFilterViewModel;

        public WindowManager(IShowDialogAgent dialogAgent,
            ILabWizardStepsProvider labWizardStepsProvider,
            IOrdersFilterViewModel ordersFilterViewModel)
        {
            _dialogAgent = dialogAgent;
            _labWizardProvider = labWizardStepsProvider;

            _ordersFilterViewModel = ordersFilterViewModel;
        }

        public Order CreateLabOrder()
        {
            var labWizard = new WizardViewModel(_labWizardProvider);
            labWizard.Initialize("Создание заказ-наряда для ЗТЛ",false, new Order());
            if (_dialogAgent.ShowDialog<WizardWindow>(labWizard) == true)
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