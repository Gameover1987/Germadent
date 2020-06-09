﻿using Germadent.Rma.App.Views.Wizard;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Operations
{
    public interface IOrderUIOperations
    {
        OrderDto CreateLabOrder(OrderDto order, WizardMode mode);

        OrderDto CreateMillingCenterOrder(OrderDto order, WizardMode mode);

        OrdersFilter CreateOrdersFilter(OrdersFilter filter);
    }
}