using System;
using Germadent.Common.Logging;
using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.Printing;
using Germadent.Rma.App.ViewModels;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockMainViewModel : MainViewModel
    {
        public DesignMockMainViewModel()
            : base(new MockRmaOperations(), new DesignMockWindowManager(), new DesignMockDialogAgent(), new DesignMockPrintModule(), new MockLogger())
        {
        }
    }

    public class DesignMockPrintModule : IPrintModule
    {
        public void Print(OrderDto order)
        {
            throw new System.NotImplementedException();
        }
    }

    public class MockLogger : ILogger
    {
        public void Info(string message)
        {
            throw new NotImplementedException();
        }

        public void Error(Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Fatal(Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
