using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;
using Germadent.Rma.Model;
using Germadent.Rma.Model.Pricing;

namespace Germadent.Rma.App.ViewModels.Pricing
{
    public interface IAddPricePositionViewModel
    {
        void Initialize(CardViewMode viewMode, PricePositionDto pricePositionDto, string[] allUserCodes, BranchType branchType);

        PricePositionDto GetPricePosition();
    }
}
