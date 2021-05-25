using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.Client.Common.Reporting;
using Germadent.Client.Common.Reporting.PropertyGrid;
using Germadent.Rms.App.ServiceClient;
using Germadent.Rms.App.ViewModels;

namespace Germadent.Rms.App.Views.DesignMock
{
    public class DesignMockFinishWorkListViewModel : FinishWorkListViewModel
    {
        public DesignMockFinishWorkListViewModel() : base(new PrintableOrderConverter(), new PropertyItemsCollector(), new DesignMockRmsServiceClient())
        {
        }
    }
}
