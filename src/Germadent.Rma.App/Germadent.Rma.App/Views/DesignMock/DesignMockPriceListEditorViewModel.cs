using System;
using Germadent.Rma.App.Infrastructure;
using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.ViewModels.Pricing;
using Germadent.UI.ViewModels.DesignTime;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockCommandExceptionHandler : ICommandExceptionHandler
    {
        public void HandleCommandException(Exception exception)
        {
            throw new NotImplementedException();
        }
    }

    public class DesignMockPriceListEditorViewModel : PriceListEditorViewModel
    {
        public DesignMockPriceListEditorViewModel() 
            : base(new DesignMockUserManager(),
                new DesignMockPriceGroupRepository(),
                new DesignMockPricePositionRepository(),
                new DesignMockShowDialogAgent(),
                new DesignMockRmaServiceClient(), 
                new DesignMockAddPricePositionViewModel(),
                new DesignMockDateTimeProvider(DateTime.Now),
                new DesignMockCommandExceptionHandler())
        {
            Initialize();
        }
    }
}