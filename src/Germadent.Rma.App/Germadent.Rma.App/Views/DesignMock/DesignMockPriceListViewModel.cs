using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.ViewModels.Pricing;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockPriceListViewModel : PriceListViewModel
    {
        public DesignMockPriceListViewModel()
            : base(new DesignMockRmaServiceClient())
        {
        }
    }
}