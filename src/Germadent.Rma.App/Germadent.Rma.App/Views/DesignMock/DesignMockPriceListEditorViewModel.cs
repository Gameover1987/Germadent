using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.ViewModels.Pricing;
using Germadent.UI.ViewModels.DesignTime;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockPriceListEditorViewModel : PriceListEditorViewModel
    {
        public DesignMockPriceListEditorViewModel() 
            : base(new DesignMockUserManager(), new DesignMockPriceGroupRepository(), new DesignMockPricePositionRepository(), new DesignMockShowDialogAgent(), new DesignMockRmaServiceClient(), new DesignMockAddPricePositionViewModel())
        {
            Initialize();
        }
    }
}