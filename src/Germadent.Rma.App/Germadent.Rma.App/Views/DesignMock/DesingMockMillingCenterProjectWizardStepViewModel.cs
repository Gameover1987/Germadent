using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.ViewModels.Wizard;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesingMockMillingCenterProjectWizardStepViewModel : MillingCenterProjectWizardStepViewModel
    {
        public DesingMockMillingCenterProjectWizardStepViewModel() 
            : base(new MockRmaOperations())
        {
        }
    }
}