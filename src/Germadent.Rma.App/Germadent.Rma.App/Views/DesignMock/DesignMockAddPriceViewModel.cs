using System;
using System.Collections.Generic;
using System.Text;
using Germadent.Rma.App.ViewModels.Pricing;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockAddPriceViewModel : AddPriceViewModel
    {
        public DesignMockAddPriceViewModel()
        {
            DateBeginning = DateTime.Now;
            PriceStl = 100;
            PriceModel = 200;
        }
    }
}
