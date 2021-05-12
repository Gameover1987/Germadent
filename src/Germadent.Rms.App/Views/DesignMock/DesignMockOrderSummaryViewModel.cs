using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.Client.Common.Reporting;
using Germadent.Client.Common.Reporting.PropertyGrid;
using Germadent.Model;
using Germadent.Rms.App.ViewModels;

namespace Germadent.Rms.App.Views.DesignMock
{
    public class DesignMockOrderSummaryViewModel : OrderSummaryViewModel
    {
        public DesignMockOrderSummaryViewModel() : base(new PrintableOrderConverter(), new PropertyItemsCollector())
        {
            Initialize(new OrderDto(){BranchType = BranchType.MillingCenter});
        }
    }
}
