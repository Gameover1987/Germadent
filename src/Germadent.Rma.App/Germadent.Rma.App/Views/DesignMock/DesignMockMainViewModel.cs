﻿using System;
using System.Collections.Generic;
using System.Linq;
using Germadent.Client.Common.DesignMock;
using Germadent.Client.Common.Infrastructure;
using Germadent.Client.Common.Reporting;
using Germadent.Client.Common.ServiceClient;
using Germadent.Client.Common.ServiceClient.Repository;
using Germadent.Common.Extensions;
using Germadent.Common.Logging;
using Germadent.Model;
using Germadent.Model.Production;
using Germadent.Rma.App.Infrastructure;
using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.App.ViewModels;
using Germadent.Rma.App.ViewModels.TechnologyOperation;
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

    public class DesignMockUserSettingsManager : IRmaUserSettingsManager
    {
        public DesignMockUserSettingsManager()
        {
            LastLogin = "Vasya";
            UserNames = new List<string>();
            Columns = new ColumnInfo[0];
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public string LastLogin { get; set; }
        public List<string> UserNames { get; set; }
        public ColumnInfo[] Columns { get; set; }
        public void Save()
        {
            throw new NotImplementedException();
        }
    }

    public class DesignMockEmployeePositionRepository : IEmployeePositionRepository
    {
        public void Initialize()
        {
            
        }

        

        public EmployeePositionDto[] Items
        {
            get
            {
                var mockServiceClient = new DesignMockRmaServiceClient();
                return mockServiceClient.GetEmployeePositions();
            }
        }

        public event EventHandler<RepositoryChangedEventArgs<EmployeePositionDto>> Changed;
    }

    public class DesignMockTechnologyOperationsEditorViewModel : TechnologyOperationsEditorViewModel
    {
        public DesignMockTechnologyOperationsEditorViewModel() : base(new DesignMockEmployeePositionRepository(), new DesignMockTechnologyOperationRepository(), new DesignMockShowDialogAgent(), new DesignMockAddTechnologyOperationViewModel())
        {
            Initialize();

            SelectedEmployeePosition = EmployeePositions.FirstOrDefault();
        }
    }

    public class DesignMockTechnologyOperationRepository : ITechnologyOperationRepository
    {
        public void Initialize()
        {
            
        }

        
        public TechnologyOperationDto[] Items
        {
            get
            {
                var mockServiceClient = new DesignMockRmaServiceClient();
                return mockServiceClient.GetTechnologyOperations();
            }
        }

        public event EventHandler<RepositoryChangedEventArgs<TechnologyOperationDto>> Changed;

        public void AddTechnologyOperation(TechnologyOperationDto technologyOperationDto)
        {
            throw new NotImplementedException();
        }

        public void EditTechnologyOperation(TechnologyOperationDto technologyOperationDto)
        {
            throw new NotImplementedException();
        }

        public void DeleteTechnologyOperation(int technologyOperationId)
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
                new DesignMockTechnologyOperationsEditorViewModel(), 
                new DesignMockPrintModule(), 
                new MockLogger(),
                new DesignMockUserManager(),
                new DesignMockUserSettingsManager(),
                new ClipboardHelper(),
                new DesignMockSignalRClient(),
                new DesignMockSalaryCalculationViewModel())
        {
            ThreadTaskExtensions.IsSyncRun = true;

            Initialize();
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
