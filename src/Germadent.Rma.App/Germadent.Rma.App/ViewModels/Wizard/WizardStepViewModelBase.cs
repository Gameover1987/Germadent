using System.Collections.ObjectModel;
using Germadent.Rma.Model;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public abstract class WizardStepViewModelBase : ViewModelBase, IWizardStepViewModel
    {
        protected WizardStepViewModelBase()
        {
            ValidationErrors = new ObservableCollection<string>();
        }

        public ObservableCollection<string> ValidationErrors { get; }

        public bool IsValid => ValidationErrors.Count == 0;

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            OnPropertyChangedImpl(nameof(this.IsValid));
        }

        public abstract string DisplayName { get; }
        public abstract void Initialize(OrderDto order);
        public abstract void AssemblyOrder(OrderDto order);
    }
}