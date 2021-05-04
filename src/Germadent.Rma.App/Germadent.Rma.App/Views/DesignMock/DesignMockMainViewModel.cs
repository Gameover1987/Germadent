using System;
using System.Linq;
using Germadent.Common.Logging;
using Germadent.Rma.App.Infrastructure;
using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.Reporting;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.App.ViewModels;
using Germadent.Rma.App.ViewModels.TechnologyOperation;
using Germadent.Rma.Model;
using Germadent.Rma.Model.Production;
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
