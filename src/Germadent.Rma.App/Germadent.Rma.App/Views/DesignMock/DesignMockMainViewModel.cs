using System;
using Germadent.Common.Logging;
using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.Reporting;
using Germadent.Rma.App.ViewModels;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockMainViewModel : MainViewModel
    {
        public DesignMockMainViewModel()
            : base(new DesignMockRmaServiceClient(), new DesignMockWindowManager(), new DesignMockDialogAgent(), new DesignMockCustomerCatalogViewModel(), new DesignMockResponsiblePersonsCatalogViewModel(),   new DesignMockPrintModule(), new MockLogger(), new ClipboardReporter(new ClipboardHelper(), new DesignMockRmaServiceClient()))
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
