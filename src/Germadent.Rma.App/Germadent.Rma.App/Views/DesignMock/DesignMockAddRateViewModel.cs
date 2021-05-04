using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.Rma.App.ViewModels.TechnologyOperation;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockAddRateViewModel : AddRateViewModel
    {
        public DesignMockAddRateViewModel()
        {
            Rate = 100;
            QualifyingRank = 2;
            DateBeginning = DateTime.Now;
        }
    }
}
