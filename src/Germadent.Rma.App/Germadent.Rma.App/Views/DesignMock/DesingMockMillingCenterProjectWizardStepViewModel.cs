﻿using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.Reporting;
using Germadent.Rma.App.ViewModels.ToothCard;
using Germadent.Rma.App.ViewModels.Wizard;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesingMockMillingCenterProjectWizardStepViewModel : MillingCenterProjectWizardStepViewModel
    {
        public DesingMockMillingCenterProjectWizardStepViewModel() 
            : base(new ToothCardViewModel(new DesignMockDictionaryRepository(), new ClipboardHelper()))
        {
            Initialize(new OrderDto{WorkAccepted = true});
        }
    }
}