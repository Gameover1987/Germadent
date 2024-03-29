﻿using Germadent.Model;
using Germadent.Rma.App.ViewModels.Wizard;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockLaboratoryWizardStepsProvider : IWizardStepsProvider
    {
        public BranchType BranchType => BranchType.Laboratory;

        public IWizardStepViewModel[] GetSteps()
        {
            return new IWizardStepViewModel[]
            {
                new DesignMockLaboratoryInfoWizardStepViewModel(),
            };
        }
    }
}