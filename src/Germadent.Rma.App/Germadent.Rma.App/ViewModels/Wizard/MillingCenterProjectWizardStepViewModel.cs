using Germadent.Common.Extensions;
using Germadent.Rma.App.ViewModels.ToothCard;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public class MillingCenterProjectWizardStepViewModel : WizardStepViewModelBase, IToothCardContainer
    {
        private string _implantSystem;
        private string _carcassColor;
        private string _additionalMillingInfo;
        private string _individualAbutmentProcessing;
        private string _understaff;
        private bool _workAccepted;
        private string _prostheticArticul;

        public MillingCenterProjectWizardStepViewModel(IToothCardViewModel toothCard, IOrderFilesContainerViewModel orderFilesContainer)
        {
            ToothCard = toothCard;
            FilesContainer = orderFilesContainer;
        }

        public override bool IsValid => !HasErrors && ToothCard.IsValid;

        public override string DisplayName
        {
            get { return "Проект"; }
        }

        public string AdditionalMillingInfo
        {
            get { return _additionalMillingInfo; }
            set { SetProperty(() => _additionalMillingInfo, value); }
        }

        public string ImplantSystem
        {
            get { return _implantSystem; }
            set { SetProperty(() => _implantSystem, value); }
        }

        public string CarcassColor
        {
            get { return _carcassColor; }
            set { SetProperty(() => _carcassColor, value); }
        }
       
        public string IndividualAbutmentProcessing
        {
            get { return _individualAbutmentProcessing; }
            set { SetProperty(() => _individualAbutmentProcessing, value); }
        }

        public string Understaff
        {
            get { return _understaff; }
            set { SetProperty(() => _understaff, value); }
        }

        public bool WorkAccepted
        {
            get { return _workAccepted; }
            set { SetProperty(() => _workAccepted, value); }
        }

        public string ProstheticArticul
        {
            get { return _prostheticArticul; }
            set { SetProperty(() => _prostheticArticul, value); }
        }

        public IToothCardViewModel ToothCard { get; } 

        public IOrderFilesContainerViewModel FilesContainer { get; }

        public override void Initialize(OrderDto order)
        {
            _additionalMillingInfo = order.AdditionalInfo;
            _implantSystem = order.ImplantSystem;
            _carcassColor = order.CarcassColor;
            _individualAbutmentProcessing = order.IndividualAbutmentProcessing;
            _understaff = order.Understaff;
            _workAccepted = order.WorkAccepted;
            _prostheticArticul = order.ProstheticArticul;

            ToothCard.Initialize(order.ToothCard);
            FilesContainer.Initialize(order);

            OnPropertyChanged();
        }

        public override void AssemblyOrder(OrderDto order)
        {
            order.AdditionalInfo = AdditionalMillingInfo;
            order.ImplantSystem = ImplantSystem;
            order.CarcassColor = CarcassColor;
            order.IndividualAbutmentProcessing = IndividualAbutmentProcessing;
            order.Understaff = Understaff;
            order.WorkAccepted = WorkAccepted;
            order.ProstheticArticul = ProstheticArticul;

            order.ToothCard = ToothCard.ToDto();
            order.ToothCard.ForEach(x => x.WorkOrderId = order.WorkOrderId);

            FilesContainer.AssemblyOrder(order);
        }
    }
}