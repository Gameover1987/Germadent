using Germadent.Rma.App.Reporting;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ViewModels;
using Germadent.Rma.App.ViewModels.Wizard;
using Germadent.Rma.App.Views;
using Germadent.Rma.App.Views.Wizard;
using Germadent.Rma.Model;
using Germadent.UI.Infrastructure;

namespace Germadent.Rma.App.Infrastructure
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
            //TODO Nekrasov: проверки на нул
            _dialogAgent = dialogAgent;
            _rmaOperations = rmaOperations;
            _labWizardProvider = labWizardStepsProvider;
            _millingCenterWizardStepsProvider = millingCenterWizardStepsProvider;

            _ordersFilterViewModel = ordersFilterViewModel;
            _printModule = printModule;
        }

        public OrderDto CreateLabOrder(OrderDto order, WizardMode mode)
        {
            // TODO: Virtual method or factory
            var labWizard = CreateWizard(_labWizardProvider);
            labWizard.Initialize(mode, order);
            //TODO Nekrasov: инвертировать
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
                    _printModule.Print(_rmaOperations.GetOrderById(changedOrder.WorkOrderId));

                return changedOrder;
            }

            return null;
        }

        public OrderDto CreateMillingCenterOrder(OrderDto order, WizardMode mode)
        {
            // TODO: Virtual method or factory
            var millingCenterWizard = CreateWizard(_millingCenterWizardStepsProvider);
            millingCenterWizard.Initialize(mode, order);
            if (_dialogAgent.ShowDialog<WizardWindow>(millingCenterWizard) != true) 
                return null;

            var changedOrder = millingCenterWizard.GetOrder();
            //TODO Nekrasov:а если мод view?
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
        //TODO Nekrasov:в конец класса
        protected virtual IWizardViewModel CreateWizard(IWizardStepsProvider stepsProvider)
        {
            return new WizardViewModel(stepsProvider, _printModule);
        }

        public OrdersFilter CreateOrdersFilter()
        {
            //TODO Nekrasov: инверти ровать
            if (_dialogAgent.ShowDialog<OrdersFilterWindow>(_ordersFilterViewModel) == true)
            {
                return _ordersFilterViewModel.GetFilter();
            }

            return null;
        }
    }
}