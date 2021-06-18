using System;
using System.Linq;
using Germadent.Model;
using Germadent.Model.Pricing;
using Germadent.Rma.App.ViewModels.Pricing;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;
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
                    new PriceDto{DateBeginning = new DateTime(2020, 10,1), PriceModel = 10, PriceStl = 22},
                    new PriceDto{DateBeginning = new DateTime(2020, 11,1), PriceModel = 11, PriceStl = 33},
                    new PriceDto{DateBeginning = new DateTime(2020, 12,1), PriceModel = 12, PriceStl = 44},
                    new PriceDto{DateBeginning = new DateTime(2021, 1,1), PriceModel = 15, PriceStl = 55},
                }
            }, new []{"111","222"}, BranchType.Laboratory);

            SelectedPrice = Prices.FirstOrDefault();
        }
    }
}
