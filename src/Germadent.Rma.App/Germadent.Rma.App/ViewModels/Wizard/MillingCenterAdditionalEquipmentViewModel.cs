using System.Collections.ObjectModel;
using Germadent.Common.Extensions;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public class MillingCenterAdditionalEquipmentViewModel : WizardStepViewModelBase
    {
        private readonly IRmaOperations _rmaOperations;

        public MillingCenterAdditionalEquipmentViewModel( IRmaOperations rmaOperations)
        {
            _rmaOperations = rmaOperations;
        }

        public override string DisplayName
        {
            get { return "Дополнительная оснастка"; }
        }

        public override bool IsValid
        {
            get { return !HasErrors; }
        }

        public override void Initialize(OrderDto order)
        {
            var equipments = _rmaOperations.GetEquipments();
            equipments.ForEach(x => Equipments.Add(x));
        }

        public override void AssemblyOrder(OrderDto order)
        {
            
        }

        public ObservableCollection<EquipmentDto> Equipments { get; } = new ObservableCollection<EquipmentDto>();
    }
}
