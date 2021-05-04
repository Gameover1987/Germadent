using System.Linq;
using Germadent.Model;
using Germadent.Rma.App.ViewModels.Pricing;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockPriceListViewModel : PriceListViewModel
    {
        public DesignMockPriceListViewModel()
            : base(new DesignMockPriceGroupRepository(), new DesignMockProductRepository())
        {
            Initialize(BranchType.Laboratory);

            SelectedGroup = Groups.First();

            SelectedGroup.HasChanges = true;
        }
    }
}