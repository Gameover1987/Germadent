using System.Collections.ObjectModel;
using System.Linq;
using Germadent.Common.Extensions;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public class LaboratoryProjectWizardStepViewModel : WizardStepViewModelBase
    {
        private readonly IDictionaryRepository _dictionaryRepository;
        private string _workDescription;
        private string _prostheticArticul;

        private DictionaryItemDto _selectedTransparency;

        public LaboratoryProjectWizardStepViewModel(IDictionaryRepository dictionaryRepository)
        {
            _dictionaryRepository = dictionaryRepository;
        }

        public override string DisplayName => "Проект";

        public override bool IsValid => !HasErrors;

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

        public ObservableCollection<DictionaryItemDto> Transparences { get; } = new ObservableCollection<DictionaryItemDto>();

        public DictionaryItemDto SelectedTransparency
        {
            get => _selectedTransparency;
            set
            {
                if (_selectedTransparency == value)
                    return;
                _selectedTransparency = value;
                OnPropertyChanged(() => SelectedTransparency);
            }
        }

        public override void Initialize(OrderDto order)
        {
            _workDescription = order.WorkDescription;
            _prostheticArticul = order.ProstheticArticul;

            Transparences.Clear();
            var transparences = _dictionaryRepository.GetItems(DictionaryType.Transparency);
            transparences.ForEach(x => Transparences.Add(x));

            SelectedTransparency = Transparences.First(x => x.Id == order.Transparency);

            OnPropertyChanged();
        }

        public override void AssemblyOrder(OrderDto order)
        {
            order.WorkDescription = WorkDescription;
            order.Transparency = SelectedTransparency.Id;
            order.ProstheticArticul = ProstheticArticul;
        }
    }
}