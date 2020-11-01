using Germadent.Rma.App.ViewModels.Pricing;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockPriceListViewModel : PriceListViewModel
    {
        public DesignMockPriceListViewModel()
            : base(new DesignMockPriceGroupRepository(), new DesignMockPricePositionRepository())
        {
            Initialize(BranchType.Laboratory);
        }
    }
}