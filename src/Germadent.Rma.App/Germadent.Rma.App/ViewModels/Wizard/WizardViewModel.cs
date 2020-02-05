using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Germadent.Rma.App.Printing;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.Views;
using Germadent.Rma.Model;
using Germadent.UI.Commands;
using Germadent.UI.Infrastructure;
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
        private readonly IPrintModule _printModule;
        private IWizardStepViewModel _currentStep;
        private BranchType _branchType;

        private OrderDto _order;

        public WizardViewModel(IWizardStepsProvider stepsProvider, IPrintModule printModule)
        {
            _printModule = printModule;
            if (stepsProvider is ILabWizardStepsProvider)
                _branchType = BranchType.Laboratory;
            else
                _branchType = BranchType.MillingCenter;

            Steps = new ObservableCollection<IWizardStepViewModel>(stepsProvider.GetSteps());
            CurrentStep = Steps.First();

            BackCommand = new DelegateCommand(x => BackCommandHandler(), x => CanBackCommandHandler());
            NextCommand = new DelegateCommand(x => NextCommandHandler(), x => CanNextCommandHandler());

            PrintCommand = new DelegateCommand(x => PrintCommandHandler());
            SaveCommand = new DelegateCommand(x => SaveCommandHandler(), x => CanSaveCommandHandler());
            SaveAndPrintCommand = new DelegateCommand(x => PrintAndSaveCommandHandler(x), x => CanPrintAndSaveCommandHandler());
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

        public ICommand PrintCommand { get; }

        public ICommand SaveAndPrintCommand { get; }

        public ICommand SaveCommand { get; }

        private bool CanBackCommandHandler()
        {
            return CurrentStep != Steps.First() && CurrentStep.IsValid;
        }

        private void BackCommandHandler()
        {
            var currenIndex = Steps.IndexOf(CurrentStep);
            CurrentStep = Steps[currenIndex - 1];
        }

        private bool CanNextCommandHandler()
        {
            return Steps.Last() != CurrentStep && CurrentStep.IsValid;
        }

        private void NextCommandHandler()
        {
            var currenIndex = Steps.IndexOf(CurrentStep);
            CurrentStep = null;
            CurrentStep = Steps[currenIndex + 1];
        }

        private bool CanPrintAndSaveCommandHandler()
        {
            return Steps.All(x => x.IsValid);
        }

        private void PrintAndSaveCommandHandler(object obj)
        {
            _printModule.Print(GetOrder());

            var window = (IWindow)obj;
            window.Close();
        }

        private bool CanSaveCommandHandler()
        {
            return Steps.All(x => x.IsValid);
        }

        private void SaveCommandHandler()
        {

        }

        private void PrintCommandHandler()
        {
            _printModule.Print(GetOrder());
        }

        public void Initialize(WizardMode wizardMode, OrderDto order)
        {
            var branchName = _branchType == BranchType.Laboratory ? "ЗТЛ" : "ФЦ";

            if (wizardMode == WizardMode.Create)
            {
                Title = "Создание заказ наряда " + branchName;
            }
            else if (wizardMode == WizardMode.Edit)
            {
                Title = $"Редактирование данных заказ наряда №'{order.DocNumber}' для {branchName}";
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
                order.DocNumber = _order.DocNumber;
                order.Closed = _order.Closed;
            }

            return order;
        }
    }
}
