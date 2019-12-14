using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Germadent.Rma.App.Views;
using Germadent.Rma.Model;
using Germadent.UI.Commands;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public interface IWizardViewModel
    {
        WizardMode WizardMode { get; }

        void Initialize(WizardMode wizardMode, OrderDto order);

        OrderDto GetOrder();
    }

    public class WizardViewModel : ViewModelBase, IWizardViewModel
    {
        private IWizardStepViewModel _currentStep;
        private BranchType _branchType;

        private OrderDto _order;

        public WizardViewModel(IWizardStepsProvider stepsProvider)
        {
            if (stepsProvider is ILabWizardStepsProvider)
                _branchType = BranchType.Laboratory;
            else
                _branchType = BranchType.MillingCenter;

            Steps = new ObservableCollection<IWizardStepViewModel>(stepsProvider.GetSteps());
            CurrentStep = Steps.First();

            BackCommand = new DelegateCommand(x => BackCommandHandler(), x => CanBackCommandHandler());
            NextCommand = new DelegateCommand(x => NextCommandHandler(), x => CanNextCommandHandler());

            OKCommand = new DelegateCommand(x => OKCommandHandler(), x => CanOkCommandHandler());
        }

        public string Title { get; protected set; }

        public WizardMode WizardMode { get; private set; }

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

        public void Initialize(WizardMode wizardMode, OrderDto order)
        {
            var branchName = _branchType == BranchType.Laboratory ? "ЗТЛ" : "ФЦ";

            if (wizardMode == WizardMode.Create)
            {
                Title = "Создание заказ наряда "+ branchName;
            }
            else if (wizardMode == WizardMode.Edit)
            {
                Title = $"Редактирование данных заказ наряда №'{order.Number}' для {branchName}";
            }

            _order = order;

            foreach (var step in Steps)
            {
                step.Initialize(order);
            }

            WizardMode = wizardMode;

            OnPropertyChanged();
        }

        public OrderDto GetOrder()
        {
            var order = new OrderDto();
            foreach (var step in Steps)
            {
                step.AssemblyOrder(order);

                order.WorkOrderId = _order.WorkOrderId;
                order.BranchType = _order.BranchType;
                order.Number = _order.Number;
                order.Closed = _order.Closed;
            }

            return order;
        }
    }
}
