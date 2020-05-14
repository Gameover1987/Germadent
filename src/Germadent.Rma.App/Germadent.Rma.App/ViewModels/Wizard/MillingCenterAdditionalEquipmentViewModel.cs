using System.Collections.ObjectModel;
using System.Linq;
using Germadent.Common.Extensions;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.ViewModels.Wizard
{

    public class MillingCenterAdditionalEquipmentViewModel : WizardStepViewModelBase
    {
        private readonly IDictionaryRepository _dictionaryRepository;

        public MillingCenterAdditionalEquipmentViewModel(IDictionaryRepository dictionaryRepository)
        {
            _dictionaryRepository = dictionaryRepository;
        }

        public override string DisplayName => "Дополнительная оснастка";

        public override bool IsValid => !HasErrors;

        public override void Initialize(OrderDto order)
        {
            Equipments.Clear();
            var equipments = _dictionaryRepository.GetItems(DictionaryType.Equipment);
            foreach (var equipment in equipments)
            {
                var equipmentViewModel = new AdditionalEquipmentViewModel(equipment);
                Equipments.Add(equipmentViewModel);
            }

            foreach (var additionalEquipment in order.AdditionalEquipment)
            {
                var equipmentViewModel = Equipments.First(x => x.EquipmentId == additionalEquipment.EquipmentId);
                equipmentViewModel.Initialize(additionalEquipment);                
            }
        }

        public override void AssemblyOrder(OrderDto order)
        {
            var equipmentsByOrder = Equipments.Select(x => x.ToDto()).ToArray();
            equipmentsByOrder.ForEach(x => x.WorkOrderId = order.WorkOrderId);
            order.AdditionalEquipment = equipmentsByOrder;
        }

        public ObservableCollection<AdditionalEquipmentViewModel> Equipments { get; } = new ObservableCollection<AdditionalEquipmentViewModel>();
    }
}
