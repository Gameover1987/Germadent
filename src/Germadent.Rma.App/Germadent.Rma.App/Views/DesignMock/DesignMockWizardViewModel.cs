using Germadent.Client.Common.DesignMock;
using Germadent.Rma.App.ViewModels.Wizard;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockWizardViewModel : WizardViewModel
    {
        public DesignMockWizardViewModel() 
            : base(new DesignMockLaboratoryWizardStepsProvider(), new DesignMockPrintModule())
        {
        }
    }
}
