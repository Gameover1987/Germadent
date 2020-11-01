using System;
using Germadent.Common.Logging;
using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.Reporting;
using Germadent.Rma.App.ViewModels;
using Germadent.Rma.App.ViewModels.Pricing;
using Germadent.Rma.Model;
using Germadent.UI.Infrastructure;
using Germadent.UI.ViewModels.DesignTime;
using Germadent.UserManagementCenter.Model;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockUserManager : IUserManager
    {
        public bool HasRight(string rightName)
        {
            throw new NotImplementedException();
        }

        public AuthorizationInfoDto AuthorizationInfo { get; }
    }

    public class DesignMockIPriceListEditorViewModel : PriceListEditorViewModel
    {

    }

    public class DesignMockMainViewModel : MainViewModel
    {
        public DesignMockMainViewModel()
            : base(new DesignMockRmaServiceClient(), new DesignMockWindowManager(), new DesignMockShowDialogAgent(), new DesignMockCustomerCatalogViewModel(), new DesignMockResponsiblePersonsCatalogViewModel(), new DesignMockIPriceListEditorViewModel(), new DesignMockPrintModule(), new MockLogger(), new ClipboardReporter(new ClipboardHelper(), new DesignMockRmaServiceClient()), new DesignMockUserManager())
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
