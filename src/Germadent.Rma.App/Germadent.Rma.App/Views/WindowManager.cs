using Germadent.Rma.App.Printing;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ViewModels;
using Germadent.Rma.App.ViewModels.Wizard;
using Germadent.Rma.Model;
using Germadent.UI.Infrastructure;

namespace Germadent.Rma.App.Views
{
    public class WindowManager : IWindowManager
    {
        private readonly IShowDialogAgent _dialogAgent;
        private readonly IRmaOperations _rmaOperations;
        private readonly ILabWizardStepsProvider _labWizardProvider;
        private readonly IMillingCenterWizardStepsProvider _millingCenterWizardStepsProvider;

        private readonly IOrdersFilterViewModel _ordersFilterViewModel;
        private readonly IPrintModule _printModule;

        public WindowManager(IShowDialogAgent dialogAgent,
            IRmaOperations rmaOperations,
            ILabWizardStepsProvider labWizardStepsProvider,
            IMillingCenterWizardStepsProvider millingCenterWizardStepsProvider,
            IOrdersFilterViewModel ordersFilterViewModel,
            IPrintModule printModule)
        {
            _dialogAgent = dialogAgent;
            _rmaOperations = rmaOperations;
            _labWizardProvider = labWizardStepsProvider;
            _millingCenterWizardStepsProvider = millingCenterWizardStepsProvider;

            _ordersFilterViewModel = ordersFilterViewModel;
            _printModule = printModule;
        }

        public OrderDto CreateLabOrder(OrderDto order, WizardMode mode)
        {
            var labWizard = new WizardViewModel(_labWizardProvider, _printModule);
            labWizard.Initialize(mode, order);
            if (_dialogAgent.ShowDialog<WizardWindow>(labWizard) == true)
            {
                var changedOrder = labWizard.GetOrder();
                if (mode == WizardMode.Create)
                {
                    changedOrder = _rmaOperations.AddOrder(changedOrder);
                }
                else
                {
                    changedOrder = _rmaOperations.UpdateOrder(changedOrder);
                }

                if (labWizard.PrintAfterSave)
                    _printModule.Print(_rmaOperations.GetOrderDetails(changedOrder.WorkOrderId));

                return changedOrder;
            }

            return null;
        }

        public OrderDto CreateMillingCenterOrder(OrderDto order, WizardMode mode)
        {
            var millingCenterWizard = new WizardViewModel(_millingCenterWizardStepsProvider, _printModule);
            millingCenterWizard.Initialize(mode, order);
            if (_dialogAgent.ShowDialog<WizardWindow>(millingCenterWizard) == true)
            {
                var changedOrder = millingCenterWizard.GetOrder();
                if (mode == WizardMode.Create)
                {
                    changedOrder = _rmaOperations.AddOrder(changedOrder);
                }
                else
                {
                    changedOrder = _rmaOperations.UpdateOrder(changedOrder);
                }

                if (millingCenterWizard.PrintAfterSave)
                    _printModule.Print(_rmaOperations.GetOrderDetails(changedOrder.WorkOrderId));

                return changedOrder;
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