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

        public MillingCenterProjectWizardStepViewModel(IToothCardViewModel toothCard)
        {
            ToothCard = toothCard;
        }

        public override bool IsValid => !HasErrors && ToothCard.IsValid;

        public override string DisplayName => "Проект";

        public string AdditionalMillingInfo
        {
            get => _additionalMillingInfo;
            set
            {
                if (_additionalMillingInfo == value)
                    return;
                _additionalMillingInfo = value;
                OnPropertyChanged(() => AdditionalMillingInfo);
            }
        }

        public string ImplantSystem
        {
            get => _implantSystem;
            set
            {
                if (_implantSystem == value)
                    return;
                _implantSystem = value;
                OnPropertyChanged(() => ImplantSystem);
            }
        }

        public string CarcassColor
        {
            get => _carcassColor;
            set
            {
                if (_carcassColor == value)
                    return;
                _carcassColor = value;
                OnPropertyChanged(() => CarcassColor);
            }
        }

        public string IndividualAbutmentProcessing
        {
            get => _individualAbutmentProcessing;
            set
            {
                if (_individualAbutmentProcessing == value)
                    return;
                _individualAbutmentProcessing = value;
                OnPropertyChanged(() => IndividualAbutmentProcessing);
            }
        }

        public string Understaff
        {
            get => _understaff;
            set
            {
                if (_understaff == value)
                    return;
                _understaff = value;
                OnPropertyChanged(() => Understaff);
            }
        }

        public bool WorkAccepted
        {
            get => _workAccepted;
            set
            {
                if (_workAccepted == value)
                    return;
                _workAccepted = value;
                OnPropertyChanged(() => WorkAccepted);
            }
        }

        public string ProstheticArticul
        {
            get => _prostheticArticul;
            set
            {
                if (_prostheticArticul == value)
                    return;
                _prostheticArticul = value;
                OnPropertyChanged(() => ProstheticArticul);
            }
        }

        public IToothCardViewModel ToothCard { get; }

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
        }
    }
}