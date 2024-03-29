﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Germadent.Client.Common.Reporting;
using Germadent.Model;
using Germadent.Rma.App.Views.Wizard;
using Germadent.UI.Commands;
using Germadent.UI.Infrastructure;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public interface IWizardViewModel
    {
        void Initialize(WizardMode wizardMode, OrderDto order);

        OrderDto GetOrder();

        bool PrintAfterSave { get; set; }
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
            _branchType = stepsProvider.BranchType;

            Steps = new ObservableCollection<IWizardStepViewModel>(stepsProvider.GetSteps());
            CurrentStep = Steps.First();

            BackCommand = new DelegateCommand(x => BackCommandHandler(), x => CanBackCommandHandler());
            NextCommand = new DelegateCommand(x => NextCommandHandler(), x => CanNextCommandHandler());
            PrintCommand = new DelegateCommand(x => PrintCommandHandler(), x => CanPrintCommandHandler());
            SaveCommand = new DelegateCommand(x => SaveCommandHandler(), x => CanSaveCommandHandler());
            SaveAndPrintCommand = new DelegateCommand(x => SaveAndPrintCommandHandler(x), x => CanSaveAndPrintCommandHandler());
        }

        public string Title { get; protected set; }

        public WizardMode WizardMode { get; private set; }

        public ObservableCollection<IWizardStepViewModel> Steps { get; }

        public IWizardStepViewModel CurrentStep
        {
            get => _currentStep;
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

        public bool IsReadOnly => WizardMode == WizardMode.View;

        public bool PrintAfterSave { get; set; }

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

        private bool CanSaveAndPrintCommandHandler()
        {
            if (IsReadOnly)
                return false;

            return Steps.All(x => x.IsValid);
        }

        private void SaveAndPrintCommandHandler(object obj)
        {
            PrintAfterSave = true;

            var window = (IWindow)obj;
            window.DialogResult = true;
        }

        private bool CanSaveCommandHandler()
        {
            if (IsReadOnly)
                return false;

            return Steps.All(x => x.IsValid);
        }

        private void SaveCommandHandler()
        {

        }

        private bool CanPrintCommandHandler()
        {
            return WizardMode == WizardMode.View;
        }

        private void PrintCommandHandler()
        {
            _printModule.PrintOrder(_order);
        }

        public void Initialize(WizardMode wizardMode, OrderDto order)
        {
            PrintAfterSave = false;

            var branchName = _branchType == BranchType.Laboratory ? "ЗТЛ" : "ФЦ";

            if (wizardMode == WizardMode.Create)
            {
                Title = "Создание заказ наряда " + branchName;
            }
            else if (wizardMode == WizardMode.Edit)
            {
                Title = $"Редактирование данных заказ наряда №'{order.DocNumber}' для {branchName}";
            }
            else if (wizardMode == WizardMode.View)
            {
                Title = $"Просмотр данных заказ наряда №'{order.DocNumber}' для {branchName}";
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

            order.WorkOrderId = _order.WorkOrderId;
            order.BranchType = _order.BranchType;
            order.DocNumber = _order.DocNumber;
            order.CreatorFullName = _order.CreatorFullName;
            order.Status = _order.Status;
            order.StatusChanged = _order.StatusChanged;
            order.LockDate = _order.LockDate;
            order.LockedBy = _order.LockedBy;

            foreach (var step in Steps)
            {
                step.AssemblyOrder(order);
            }

            return order;
        }
    }
}
