using System.Collections.ObjectModel;
using System.Linq;
using Germadent.Client.Common.ServiceClient;
using Germadent.Client.Common.ServiceClient.Repository;
using Germadent.Common.Extensions;
using Germadent.Model;
using Germadent.Rma.App.ServiceClient.Repository;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public class AdditionalEquipmentWizardStepViewModel : WizardStepViewModelBase
    {
        private readonly IDictionaryRepository _dictionaryRepository;
        private readonly IAttributeRepository _attributeRepository;

        private string _workDescription;
        private bool _workAccepted;

        public AdditionalEquipmentWizardStepViewModel(IDictionaryRepository dictionaryRepository, IAttributeRepository attributeRepository)
        {
            _dictionaryRepository = dictionaryRepository;
            _attributeRepository = attributeRepository;
        }

        public override string DisplayName => "Описание работ";

        public override bool IsValid => !HasErrors;

        public ObservableCollection<AdditionalEquipmentViewModel> Equipments { get; } = new ObservableCollection<AdditionalEquipmentViewModel>();

        public ObservableCollection<AttributeViewModel> Attributes { get; } = new ObservableCollection<AttributeViewModel>();
        
        public string WorkDescription
        {
            get => _workDescription;
            set
            {
                if (_workDescription == value)
                    return;
                _workDescription = value;
                OnPropertyChanged(() => WorkDescription);
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

            var attributeGroups = _attributeRepository.Items.GroupBy(x => x.AttributeName).ToArray();
            Attributes.Clear();
            foreach (var attributeGroup in attributeGroups)
            {
                var selectedValue =
                    order.Attributes.FirstOrDefault(x => x.AttributeKeyName == attributeGroup.First().AttributeKeyName);
                
                var attributeViewModel = new AttributeViewModel(attributeGroup.First().AttributeName, selectedValue == null ? 0 : selectedValue.AttributeValueId, attributeGroup.ToArray());
                Attributes.Add(attributeViewModel);
            }

            _workAccepted = order.WorkAccepted;
            _workDescription = order.WorkDescription;
        }

        public override void AssemblyOrder(OrderDto order)
        {
            order.AdditionalEquipment = Equipments.Select(x => x.ToDto()).ToArray();
            order.AdditionalEquipment.ForEach(x => x.WorkOrderId = order.WorkOrderId);
            order.WorkDescription = WorkDescription;
            order.WorkAccepted = WorkAccepted;
            order.Attributes = Attributes.Where(x => x.SelectedValue != null).Select(x => x.SelectedValue).ToArray();
            order.Attributes.ForEach(x => x.WorkOrderId = order.WorkOrderId);
        }
    }
}
