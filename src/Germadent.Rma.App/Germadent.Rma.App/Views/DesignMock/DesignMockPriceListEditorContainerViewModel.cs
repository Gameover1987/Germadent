using System;
using System.Collections.Generic;
using System.Text;
using Germadent.Rma.App.ViewModels.Pricing;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockPriceListEditorFactory : IPriceListEditorFactory
    {
        public IPriceListEditorViewModel CreateEditor(BranchType branchType)
        {
            return new DesignMockPriceListEditorViewModel() {BranchType = branchType};
        }
    }

    public class DesignMockPriceListEditorContainerViewModel : PriceListEditorContainerViewModel
    {
        public DesignMockPriceListEditorContainerViewModel()
            : base(new DesignMockPriceListEditorFactory())
        {
            Initialize();
        }
    }
}
