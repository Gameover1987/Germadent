using Germadent.Rma.Model;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public interface IWizardStepsProvider 
    {
        IWizardStepViewModel[] GetSteps();
    }
}