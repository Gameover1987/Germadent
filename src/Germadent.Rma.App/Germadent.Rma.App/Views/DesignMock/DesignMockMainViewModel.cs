﻿using System;
using Germadent.Common.Logging;
using Germadent.Rma.App.Infrastructure;
using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.Reporting;
using Germadent.Rma.App.ViewModels;
using Germadent.Rma.Model;
using Germadent.UI.Infrastructure;
using Germadent.UI.ViewModels.DesignTime;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockEnvironment : IEnvironment
    {
        public void Restart()
        {
            
        }

        public void Shutdown()
        {
            
        }
    }

    public class DesignMockUserSettingsManager : IUserSettingsManager
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public string LastLogin { get; set; }
        public ColumnInfo[] Columns { get; set; }
        public void Save()
        {
            throw new NotImplementedException();
        }
    }

    public class DesignMockMainViewModel : MainViewModel
    {
        public DesignMockMainViewModel()
            : base(new DesignMockRmaServiceClient(),
                new DesignMockEnvironment(), 
                new DesignMockWindowManager(), 
                new DesignMockShowDialogAgent(), 
                new DesignMockCustomerCatalogViewModel(),
                new DesignMockResponsiblePersonsCatalogViewModel(),
                new DesignMockPriceListEditorContainerViewModel(), 
                new DesignMockPrintModule(), 
                new MockLogger(), 
                new ClipboardReporter(new ClipboardHelper(), new DesignMockRmaServiceClient()), 
                new DesignMockUserManager(),
                new DesignMockUserSettingsManager() )
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
