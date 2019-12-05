using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.Printing;
using Germadent.Rma.App.ViewModels;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockMainViewModel : MainViewModel
    {
        public DesignMockMainViewModel()
            : base(new MockRmaOperations(), new DesignMockWindowManager(), new DesignMockDialogAgent(), new DesignMockPrintModule())
        {
        }
    }

    public class DesignMockPrintModule : IPrintModule
    {
        public void Print(Order order)
        {
            throw new System.NotImplementedException();
        }
    }
}
