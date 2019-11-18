using Germadent.Rma.Model;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public interface IWizardStepViewModel
    {
        string DisplayName { get; }

        void Initialize(Order order);
    }
}