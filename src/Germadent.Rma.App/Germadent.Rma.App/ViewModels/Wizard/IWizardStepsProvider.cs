using Germadent.Model;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public interface IWizardStepsProvider 
    {
        BranchType BranchType { get; }

        IWizardStepViewModel[] GetSteps();
    }
}