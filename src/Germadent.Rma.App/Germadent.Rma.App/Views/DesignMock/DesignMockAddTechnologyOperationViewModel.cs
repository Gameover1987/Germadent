using System;
using Germadent.Model.Production;
using Germadent.Rma.App.ViewModels.TechnologyOperation;
using Germadent.UI.ViewModels.DesignTime;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockAddTechnologyOperationViewModel : AddTechnologyOperationViewModel
    {
        public DesignMockAddTechnologyOperationViewModel() : base(
            new DesignMockEmployeePositionRepository(),
            new DesignMockTechnologyOperationRepository(), 
            new DesignMockPricePositionRepository(),
            new DesignMockAddRateViewModel(), 
            new DesignMockShowDialogAgent())
        {
            Initialize(ViewMode.Add, new TechnologyOperationDto
            {
                Name = "Patch KDE 2.0 to FreeBSD",
                Rates = new RateDto[]
                {
                    new RateDto{DateBeginning = DateTime.Now.AddDays(-3), Rate = 100, QualifyingRank = 1},
                    new RateDto{DateBeginning = DateTime.Now, Rate = 200, QualifyingRank = 2},
                    new RateDto{DateBeginning = DateTime.Now.AddDays(8), Rate = 300, QualifyingRank = 3},
                },
                UserCode = "222333444",
                IsObsolete = true
            });
        }
    }
}
