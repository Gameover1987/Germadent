using Germadent.Rma.Model;
using Germadent.UI.ViewModels.Validation;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public abstract class WizardStepViewModelBase : ValidationSupportableViewModel, IWizardStepViewModel
    {
        protected WizardStepViewModelBase()
        {

        }

        public abstract string DisplayName { get; }
        public abstract bool IsValid { get; }
        public abstract void Initialize(OrderDto order);
        public abstract void AssemblyOrder(OrderDto order);
    }
}