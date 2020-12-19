﻿using Germadent.Rma.App.Reporting;
using Germadent.Rma.App.ViewModels.ToothCard;
using Germadent.Rma.App.ViewModels.Wizard;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockPriceListWizardStepViewModel : PriceListWizardStepViewModel
    {
        public DesignMockPriceListWizardStepViewModel() 
            : base(new ToothCardViewModel(new DesignMockDictionaryRepository(), new DesignMockProductRepository(), new ClipboardHelper()), new DesignMockPriceListViewModel())
        {
        }
    }
}
