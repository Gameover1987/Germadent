﻿using Germadent.Rma.Model;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockWindowManager : IWindowManager
    {
        public OrderDto CreateLabOrder(OrderDto order, WizardMode mode)
        {
            throw new System.NotImplementedException();
        }

        public OrderDto CreateMillingCenterOrder(OrderDto order, WizardMode mode)
        {
            throw new System.NotImplementedException();
        }

        public OrdersFilter CreateOrdersFilter()
        {
            throw new System.NotImplementedException();
        }
    }
}