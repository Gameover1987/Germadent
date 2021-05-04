using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Germadent.Model;
using Germadent.Model.Pricing;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;

namespace Germadent.Rma.App.ViewModels.Pricing
{
    public interface IAddPricePositionViewModel
    {
        void Initialize(CardViewMode viewMode, PricePositionDto pricePositionDto, string[] allUserCodes, BranchType branchType);

        PricePositionDto GetPricePosition();
    }
}
