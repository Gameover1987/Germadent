using Germadent.Rma.App.ViewModels.Pricing;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockIPriceListEditorViewModel : PriceListEditorViewModel
    {
        public DesignMockIPriceListEditorViewModel() : base(new DesignMockPriceListUIOperations(), new DesignMockUserManager())
        {
        }
    }
}