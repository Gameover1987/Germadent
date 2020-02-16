using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ViewModels.Wizard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockMillingCenterAdditionalEquipmentViewModel : MillingCenterAdditionalEquipmentViewModel
    {
        public DesignMockMillingCenterAdditionalEquipmentViewModel()
            : base(new DesignMockRmaOperations())
        {
        }
    }
}
