using Germadent.Common.Extensions;
using Germadent.Model;
using Germadent.Rma.App.ViewModels.ToothCard;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public class MillingCenterProjectWizardStepViewModel : WizardStepViewModelBase, IToothCardContainer
    {
        private bool _workAccepted;
        private string _prostheticArticul;

        public MillingCenterProjectWizardStepViewModel(IToothCardViewModel toothCard)
        {
            ToothCard = toothCard;
        }

        public override bool IsValid => !HasErrors && ToothCard.IsValid;

        public override string DisplayName => "Проект";


        

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
            _workAccepted = order.WorkAccepted;
            _prostheticArticul = order.ProstheticArticul;

            ToothCard.Initialize(order.ToothCard);

            OnPropertyChanged();
        }

        public override void AssemblyOrder(OrderDto order)
        {
            order.ProstheticArticul = ProstheticArticul;

            order.ToothCard = ToothCard.ToDto();
            order.ToothCard.ForEach(x => x.WorkOrderId = order.WorkOrderId);
        }
    }
}