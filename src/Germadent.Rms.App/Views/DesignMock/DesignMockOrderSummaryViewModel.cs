using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.Client.Common.Reporting;
using Germadent.Client.Common.Reporting.PropertyGrid;
using Germadent.Client.Common.ServiceClient;
using Germadent.Model;
using Germadent.Rms.App.ViewModels;

namespace Germadent.Rms.App.Views.DesignMock
{
    public class DesignMockOrderSummaryViewModel : OrderSummaryViewModel
    {
        public DesignMockOrderSummaryViewModel() : base(new PrintableOrderConverter(), new PropertyItemsCollector(), new DesignMockRmsServiceClient())
        {
            Initialize(new OrderDto() { BranchType = BranchType.MillingCenter });

            foreach (var technologyOperationByUserViewModel in Operations)
            {
                technologyOperationByUserViewModel.IsChecked = true;
            }
        }
    }
}
