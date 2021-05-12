using Germadent.Client.Common.Reporting;
using Germadent.Client.Common.ViewModels;
using Germadent.Model;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ViewModels.Wizard;
using Germadent.Rma.App.Views;
using Germadent.Rma.App.Views.Wizard;
using Germadent.UI.Infrastructure;

namespace Germadent.Rma.App.Operations
{
    public class OrderUIOperations : IOrderUIOperations
    {
        private readonly IShowDialogAgent _dialogAgent;
        private readonly IRmaServiceClient _rmaOperations;
        private readonly ILabWizardStepsProvider _labWizardProvider;
        private readonly IMillingCenterWizardStepsProvider _millingCenterWizardStepsProvider;
        private readonly IOrdersFilterViewModel _ordersFilterViewModel;
        private readonly IPrintModule _printModule;

        public OrderUIOperations(IShowDialogAgent dialogAgent,
            IRmaServiceClient rmaOperations,
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
            var labWizard = CreateWizard(_labWizardProvider);
            labWizard.Initialize(mode, order);
            
            if (_dialogAgent.ShowDialog<WizardWindow>(labWizard) != true) 
                return null;
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
                _printModule.Print(_rmaOperations.GetOrderById(changedOrder.WorkOrderId));

            return changedOrder;

        }

        public OrderDto CreateMillingCenterOrder(OrderDto order, WizardMode mode)
        {
            var millingCenterWizard = CreateWizard(_millingCenterWizardStepsProvider);
            millingCenterWizard.Initialize(mode, order);
            if (_dialogAgent.ShowDialog<WizardWindow>(millingCenterWizard) != true) 
                return null;

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
                _printModule.Print(_rmaOperations.GetOrderById(changedOrder.WorkOrderId));

            return changedOrder;

        }

        public OrdersFilter CreateOrdersFilter(OrdersFilter filter)
        {
            _ordersFilterViewModel.Initialize(filter);
            if (_dialogAgent.ShowDialog<OrdersFilterWindow>(_ordersFilterViewModel) != true) 
                return null;

            return _ordersFilterViewModel.GetFilter();
        }

        protected virtual IWizardViewModel CreateWizard(IWizardStepsProvider stepsProvider)
        {
            return new WizardViewModel(stepsProvider, _printModule);
        }
    }
}