using Germadent.Rma.App.ViewModels.Pricing;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockPriceListEditorViewModel : PriceListEditorViewModel
    {
        public DesignMockPriceListEditorViewModel() : base(new DesignMockPriceListUIOperations(), new DesignMockUserManager())
        {
        }
    }
}