using System;
using Germadent.Client.Common.Infrastructure;
using Germadent.Client.Common.Reporting;
using Germadent.Client.Common.Reporting.PropertyGrid;
using Germadent.Rms.App.Infrastructure;
using Germadent.Rms.App.ServiceClient;
using Germadent.Rms.App.ViewModels;

namespace Germadent.Rms.App.Views.DesignMock
{
    public class DesignMockComandExceptionHandler : ICommandExceptionHandler
    {
        public void HandleCommandException(Exception exception)
        {
            throw new NotImplementedException();
        }
    }
    
    public class DesignMockOrderDetailsViewModel : OrderDetailsViewModel
    {
        public DesignMockOrderDetailsViewModel() 
            : base(new PrintableOrderConverter(), new PropertyItemsCollector(), new DesignMockRmsServiceClient(), new DesignMockComandExceptionHandler())
        {
        }
    }
}