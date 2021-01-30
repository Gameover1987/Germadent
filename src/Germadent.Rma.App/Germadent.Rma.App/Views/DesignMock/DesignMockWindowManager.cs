using Germadent.Rma.App.Operations;
using Germadent.Rma.App.ViewModels;
using Germadent.Rma.App.Views.Wizard;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockWindowManager : IOrderUIOperations
    {
        public OrderDto CreateLabOrder(OrderDto order, WizardMode mode)
        {
            throw new System.NotImplementedException();
        }

        public OrderDto CreateMillingCenterOrder(OrderDto order, WizardMode mode)
        {
            throw new System.NotImplementedException();
        }

        public OrdersFilter CreateOrdersFilter(OrdersFilter filter)
        {
            throw new System.NotImplementedException();
        }


        public ICustomerViewModel SelectCustomer(string mask)
        {
            throw new System.NotImplementedException();
        }

        public CustomerDto AddCustomer()
        {
            throw new System.NotImplementedException();
        }
    }
}