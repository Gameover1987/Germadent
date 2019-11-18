using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Germadent.Rma.Model;
using Germadent.UI.Commands;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public interface IWizardViewModel
    {
        bool IsReadOnly { get; }

        void Initialize(string title, bool isReadOnly, Order order);

        Order GetOrder();
    }

    public class WizardViewModel : ViewModelBase, IWizardViewModel
    {
        private IWizardStepViewModel _currentStep;

        public WizardViewModel(IWizardStepsProvider stepsProvider)
        {
            Steps = new ObservableCollection<IWizardStepViewModel>(stepsProvider.GetSteps());
            CurrentStep = Steps.First();

            BackCommand = new DelegateCommand(x => BackCommandHandler(), x => CanBackCommandHandler());
            NextCommand = new DelegateCommand(x => NextCommandHandler(), x => CanNextCommandHandler());

            OKCommand = new DelegateCommand(x => OKCommandHandler(), x => CanOkCommandHandler());
        }

        public string Title { get; protected set; }

        public bool IsReadOnly { get; private set; }

        public ObservableCollection<IWizardStepViewModel> Steps { get; }

        public IWizardStepViewModel CurrentStep
        {
            get { return _currentStep; }
            private set
            {
                if (_currentStep == value)
                    return;
                _currentStep = value;
                OnPropertyChanged(() => CurrentStep);
            }
        }

        public ICommand BackCommand { get; }
        public ICommand NextCommand { get; }

        public ICommand OKCommand { get; }

        private bool CanBackCommandHandler()
        {
            return CurrentStep != Steps.First();
        }

        private void BackCommandHandler()
        {
            var currenIndex = Steps.IndexOf(CurrentStep);
            CurrentStep = Steps[currenIndex - 1];
        }

        private bool CanNextCommandHandler()
        {
            return Steps.Last() != CurrentStep;
        }

        private void NextCommandHandler()
        {
            var currenIndex = Steps.IndexOf(CurrentStep);
            CurrentStep = null;
            CurrentStep = Steps[currenIndex + 1];
        }

        private bool CanOkCommandHandler()
        {
            return true;
        }

        private void OKCommandHandler()
        {

        }

        public void Initialize(string title, bool isReadOnly, Order order)
        {
            Title = title;

            foreach (var step in Steps)
            {
                step.Initialize(order);
            }

            IsReadOnly = isReadOnly;

            OnPropertyChanged();
        }

        public Order GetOrder()
        {
            return new Order();
        }
    }
}
