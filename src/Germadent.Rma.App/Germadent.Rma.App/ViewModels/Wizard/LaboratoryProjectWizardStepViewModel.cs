using System.Collections.ObjectModel;
using System.Linq;
using Germadent.Common.Extensions;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ViewModels.ToothCard;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public class LaboratoryProjectWizardStepViewModel : WizardStepViewModelBase, IToothCardContainer
    {
        private readonly IRmaServiceClient _rmaOperations;
        private string _workDescription;
        private string _colorAndFeatures;
        private int _transparency;
        private string _prostheticArticul;

        private DictionaryItemDto _selectedTransparency;

        public LaboratoryProjectWizardStepViewModel(IToothCardViewModel toothCard, IOrderFilesContainerViewModel orderFilesContainer, IRmaServiceClient rmaOperations)
        {
            _rmaOperations = rmaOperations;
            FilesContainer = orderFilesContainer;
            ToothCard = toothCard;
        }

        public override string DisplayName => "Проект";

        public override bool IsValid => !HasErrors && ToothCard.IsValid;

        public string WorkDescription
        {
            get { return _workDescription; }
            set { SetProperty(() => _workDescription, value); }
        }

        public string ColorAndFeatures
        {
            get { return _colorAndFeatures; }
            set { SetProperty(() => _colorAndFeatures, value); }
        }

        public string ProstheticArticul
        {
            get { return _prostheticArticul; }
            set { SetProperty(() => _prostheticArticul, value); }
        }

        public ObservableCollection<DictionaryItemDto> Transparences { get; } = new ObservableCollection<DictionaryItemDto>();

        public DictionaryItemDto SelectedTransparency
        {
            get { return _selectedTransparency; }
            set
            {
                if (_selectedTransparency == value)
                    return;
                _selectedTransparency = value;
                OnPropertyChanged(() => SelectedTransparency);
            }
        }

        public IToothCardViewModel ToothCard { get; }

        public IOrderFilesContainerViewModel FilesContainer { get; }

        public override void Initialize(OrderDto order)
        {
            _workDescription = order.WorkDescription;
            _colorAndFeatures = order.ColorAndFeatures;
            _transparency = order.Transparency;
            _prostheticArticul = order.ProstheticArticul;

            Transparences.Clear();
            var transparences = _rmaOperations.GetDictionary(DictionaryType.Transparency);
            transparences.ForEach(x => Transparences.Add(x));

            SelectedTransparency = Transparences.First(x => x.Id == order.Transparency);

            ToothCard.Initialize(order.ToothCard);
            FilesContainer.Initialize(order);

            OnPropertyChanged();
        }

        public override void AssemblyOrder(OrderDto order)
        {
            order.WorkDescription = WorkDescription;
            order.ColorAndFeatures = ColorAndFeatures;
            order.Transparency = SelectedTransparency.Id;
            order.ProstheticArticul = ProstheticArticul;

            order.ToothCard = ToothCard.ToDto();
            order.ToothCard.ForEach(x => x.WorkOrderId = order.WorkOrderId);

            FilesContainer.AssemblyOrder(order);
        }
    }
}