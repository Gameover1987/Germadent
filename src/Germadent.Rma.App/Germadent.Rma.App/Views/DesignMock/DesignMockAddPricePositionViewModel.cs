using System;
using System.Linq;
using Germadent.Rma.App.ViewModels.Pricing;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;
using Germadent.Rma.Model;
using Germadent.Rma.Model.Pricing;
using Germadent.UI.ViewModels.DesignTime;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockAddPricePositionViewModel : AddPricePositionViewModel
    {
        public DesignMockAddPricePositionViewModel() 
            : base(new DesignMockPriceGroupRepository(), 
            new DesignMockDictionaryRepository(),
            new DesignMockDateTimeProvider(new DateTime(2020,11,20)),
            new DesignMockUiTimer(),
            new DesignMockShowDialogAgent(),
            new DesignMockAddPriceViewModel())
        {
            Initialize(CardViewMode.Add, new PricePositionDto
            {
                PriceGroupId = 1,                                                                                                                       
                Name = "Preved position",
                UserCode = "MC222",
                MaterialId = 1,
                ProstheticTypeId = 1,
                Prices = new[]
                {
                    new PriceDto{DateBeginning = new DateTime(2020, 10,1), PriceModel = 10, PriceSTL = 22},
                    new PriceDto{DateBeginning = new DateTime(2020, 11,1), PriceModel = 11, PriceSTL = 33},
                    new PriceDto{DateBeginning = new DateTime(2020, 12,1), PriceModel = 12, PriceSTL = 44},
                    new PriceDto{DateBeginning = new DateTime(2021, 1,1), PriceModel = 15, PriceSTL = 55},
                }
            }, new []{"111","222"}, BranchType.Laboratory);

            SelectedPrice = Prices.FirstOrDefault();
        }
    }
}
