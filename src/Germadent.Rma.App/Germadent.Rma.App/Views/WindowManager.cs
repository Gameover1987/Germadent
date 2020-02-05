﻿using Germadent.Rma.App.Printing;
using Germadent.Rma.App.ViewModels;
using Germadent.Rma.App.ViewModels.Wizard;
using Germadent.Rma.Model;
using Germadent.UI.Infrastructure;

namespace Germadent.Rma.App.Views
{
    public class WindowManager : IWindowManager
    {
        private readonly IShowDialogAgent _dialogAgent;
        private readonly ILabWizardStepsProvider _labWizardProvider;
        private readonly IMillingCenterWizardStepsProvider _millingCenterWizardStepsProvider;

        private readonly IOrdersFilterViewModel _ordersFilterViewModel;
        private readonly IPrintModule _printModule;

        public WindowManager(IShowDialogAgent dialogAgent,
            ILabWizardStepsProvider labWizardStepsProvider,
            IMillingCenterWizardStepsProvider millingCenterWizardStepsProvider,
            IOrdersFilterViewModel ordersFilterViewModel,
            IPrintModule printModule)
        {
            _dialogAgent = dialogAgent;
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
                return labWizard.GetOrder();
            }

            return null;
        }

        public OrderDto CreateMillingCenterOrder(OrderDto order, WizardMode mode)
        {
            var millingCenterWizard = new WizardViewModel(_millingCenterWizardStepsProvider, _printModule);
            millingCenterWizard.Initialize(mode, order);
            if (_dialogAgent.ShowDialog<WizardWindow>(millingCenterWizard) == true)
            {
                return millingCenterWizard.GetOrder();
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