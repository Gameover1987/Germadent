using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.ViewModels;
using Germadent.ServiceClient.Model;
using Germadent.ServiceClient.Operation;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockMainViewModel : MainViewModel
    {
        public DesignMockMainViewModel()
            : base(new MockRmaOperations(), new DesignMockWindowManager(), new DesignMockDialogAgent())
        {
        }
    }

    public class DesignMockWindowManager : IWindowManager
    {
        public Order CreateLabOrder()
        {
            throw new System.NotImplementedException();
        }

        public Order CreateMillingCenterOrder()
        {
            throw new System.NotImplementedException();
        }

        public OrdersFilter CreateOrdersFilter()
        {
            throw new System.NotImplementedException();
        }
    }
}
