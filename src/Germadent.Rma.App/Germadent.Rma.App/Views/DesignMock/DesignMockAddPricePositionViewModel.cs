using System;
using System.Collections.Generic;
using System.Text;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.App.ViewModels.Pricing;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;
using Germadent.Rma.Model;
using Germadent.Rma.Model.Pricing;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockAddPricePositionViewModel : AddPricePositionViewModel
    {
        public DesignMockAddPricePositionViewModel() : base(new DesignMockPriceGroupRepository())
        {
            Initialize(CardViewMode.Add, new PricePositionDto
            {
                PriceGroupId = 1,
                Name = "Preved position",
                UserCode = "222",
                PriceModel = 111,
                PriceStl = 222
            }, new []{"111","222"}, BranchType.Laboratory);
        }
    }
}
