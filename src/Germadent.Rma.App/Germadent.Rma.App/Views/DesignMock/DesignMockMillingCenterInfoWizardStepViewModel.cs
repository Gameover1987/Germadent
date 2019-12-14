using Germadent.Rma.App.ViewModels.Wizard;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockMillingCenterInfoWizardStepViewModel : MillingCenterInfoWizardStepViewModel
    {
        public DesignMockMillingCenterInfoWizardStepViewModel()
        {
            Customer = "Customer";
            Patient = "PatientInfo";
            ResponsiblePerson = "ResponsiblePerson";
            ResponsiblePersonPhone = "ResponsiblePersonPhone";
        }
    }
}