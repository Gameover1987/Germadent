using System.Windows;
using Germadent.Rma.App.ViewModels;
using Germadent.ServiceClient.Model;
using Germadent.ServiceClient.Operation;
using Germadent.UI.Infrastructure;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockMainViewModel : MainViewModel
    {
        public DesignMockMainViewModel()
            : base(new MockRmaOperations(), new DesignMockWindowManager())
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

        public IOrdersFilterViewModel CreateOrdersFilter()
        {
            throw new System.NotImplementedException();
        }
    }
}
