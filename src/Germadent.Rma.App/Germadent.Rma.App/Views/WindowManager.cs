﻿using System;
using Germadent.Rma.App.ViewModels;
using Germadent.Rma.App.ViewModels.Wizard;
using Germadent.Rma.Model;
using Germadent.UI.Infrastructure;

namespace Germadent.Rma.App.Views
{
    public interface IWindowManager
    {
        Order CreateLabOrder(Order order);

        Order CreateMillingCenterOrder(Order order);

        OrdersFilter CreateOrdersFilter();
    }

    public class WindowManager : IWindowManager
    {
        private readonly IShowDialogAgent _dialogAgent;
        private readonly ILabWizardStepsProvider _labWizardProvider;
        private readonly IMillingCenterWizardStepsProvider _millingCenterWizardStepsProvider;

        private readonly IOrdersFilterViewModel _ordersFilterViewModel;

        public WindowManager(IShowDialogAgent dialogAgent,
            ILabWizardStepsProvider labWizardStepsProvider,
            IMillingCenterWizardStepsProvider millingCenterWizardStepsProvider,
            IOrdersFilterViewModel ordersFilterViewModel)
        {
            _dialogAgent = dialogAgent;
            _labWizardProvider = labWizardStepsProvider;
            _millingCenterWizardStepsProvider = millingCenterWizardStepsProvider;

            _ordersFilterViewModel = ordersFilterViewModel;
        }

        public Order CreateLabOrder(Order order)
        {
            var labWizard = new WizardViewModel(_labWizardProvider);
            labWizard.Initialize("Создание заказ-наряда для ЗТЛ", false, order);
            if (_dialogAgent.ShowDialog<WizardWindow>(labWizard) == true)
            {
                return labWizard.GetOrder();
            }

            return null;
        }

        public Order CreateMillingCenterOrder(Order order)
        {
            var millingCenterWizard = new WizardViewModel(_millingCenterWizardStepsProvider);
            millingCenterWizard.Initialize("Создание заказ-наряда для ФЦ", false, order);
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