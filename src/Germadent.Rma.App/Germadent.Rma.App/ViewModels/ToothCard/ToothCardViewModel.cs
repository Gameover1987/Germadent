using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Germadent.Common.Extensions;
using Germadent.Rma.App.Reporting;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.App.ViewModels.Pricing;
using Germadent.Rma.Model;
using Germadent.UI.Commands;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.ToothCard
{
    public class ToothCardViewModel : ViewModelBase, IToothCardViewModel
    {
        private readonly IDictionaryRepository _dictionaryRepository;
        private readonly IClipboardHelper _clipboard;
        private ToothViewModel[] _selectedTeeth;

        public ToothCardViewModel(IDictionaryRepository dictionaryRepository, IClipboardHelper clipboard)
        {
            _dictionaryRepository = dictionaryRepository;
            _clipboard = clipboard;

            Teeth = new ObservableCollection<ToothViewModel>();
            Materials = new ObservableCollection<CheckableDictionaryItemViewModel>();
            Prosthetics = new ObservableCollection<CheckableDictionaryItemViewModel>();

            SelectPtostheticsConditionCommand = new DelegateCommand(SelectProstheticConditionsCommandHandler, x => CanSelectProstheticConditionCommandHandler());
            SelectPtostheticsTypeCommand = new DelegateCommand(SelectProstheticsTypeCommandHandler, x => CanSelectTypeOfProstheticsCommandHandler());
            SelectMaterialCommand = new DelegateCommand(SelectMaterialCommandHandler, x => CanSelectMaterialCommandHandler());
            SelectBridgeCommand = new DelegateCommand(SelectBridgeCommandHandler, x => CanSelectBridgeCommandHandler());
            ClearCommand = new DelegateCommand(x => ClearCommandHandler(), x => CanClearCommandHandler());
            CopyDescriptionCommand = new DelegateCommand(x => CopyDescriptionCommandHandler(), x => CanCopyDescriptionCommandHandler());
        }

        public ObservableCollection<ToothViewModel> Teeth { get; }

        public ObservableCollection<CheckableDictionaryItemViewModel> Materials { get; }

        public ObservableCollection<CheckableDictionaryItemViewModel> Prosthetics { get; }

        public ICommand SelectPtostheticsConditionCommand { get; }

        public ICommand SelectPtostheticsTypeCommand { get; }

        public ICommand SelectMaterialCommand { get; }

        public ICommand SelectBridgeCommand { get; }

        public ICommand ClearCommand { get; }

        public ICommand CopyDescriptionCommand { get; }

        public bool IsValid
        {
            get { return Teeth.Where(x => x.IsChanged).All(x => x.IsValid); }
        }

        public event EventHandler<ToothSelectedEventArgs> ToothSelected;
        public event EventHandler<ToothCleanUpEventArgs> ToothCleanup;

        public void Initialize(ToothDto[] toothCard)
        {
            var prostheticConditions = _dictionaryRepository.GetItems(DictionaryType.ProstheticCondition);
            var materials = _dictionaryRepository.GetItems(DictionaryType.Material);
            var prosteticTypes = _dictionaryRepository.GetItems(DictionaryType.ProstheticType);

            foreach (var toothViewModel in Teeth)
            {
                toothViewModel.ToothChanged -= TeethViewModelOnToothChanged;
                toothViewModel.ToothCleanup -= ToothViewModelOnToothCleanup;
            }

            Teeth.Clear();

            for (int i = 18; i >= 11; i--)
            {
                Teeth.Add(new ToothViewModel(prostheticConditions, prosteticTypes, materials) { Number = i });
            }

            for (int i = 21; i <= 28; i++)
            {
                Teeth.Add(new ToothViewModel(prostheticConditions, prosteticTypes, materials) { Number = i });
            }

            for (int i = 38; i >= 31; i--)
            {
                Teeth.Add(new ToothViewModel(prostheticConditions, prosteticTypes, materials) { Number = i });
            }

            for (int i = 41; i <= 48; i++)
            {
                Teeth.Add(new ToothViewModel(prostheticConditions, prosteticTypes, materials) { Number = i });
            }

            foreach (var teethViewModel in Teeth)
            {
                teethViewModel.ToothChanged += TeethViewModelOnToothChanged;
                teethViewModel.ToothCleanup += ToothViewModelOnToothCleanup;

                var toothDto = toothCard.FirstOrDefault(x => x.ToothNumber == teethViewModel.Number);
                if (toothDto == null)
                    continue;

                teethViewModel.Initialize(toothDto);
            }
        }

        private void ToothViewModelOnToothCleanup(object sender, ToothCleanUpEventArgs e)
        {
            ToothCleanup?.Invoke(this, e);
        }

        public ToothDto[] ToDto()
        {
            return Teeth.Where(x => x.IsChanged).Select(x => x.ToDto()).ToArray();
        }

        public event EventHandler<ToothChangedEventArgs> ToothChanged;

        public string Description
        {
            get
            {
                var builder = new StringBuilder();

                foreach (var toothViewModel in Teeth.Where(x => x.IsChanged).OrderBy(x => x.Number))
                {
                    builder.AppendLine(string.Format("{0}", toothViewModel.Description));
                }

                return builder.ToString().Trim(new[] { ' ', ',' });
            }
        }

        public ToothViewModel[] SelectedTeeth
        {
            get => _selectedTeeth;
            set
            {
                _selectedTeeth = value;
                OnPropertyChanged(() => SelectedTeeth);

                ToothSelected?.Invoke(this, new ToothSelectedEventArgs(_selectedTeeth?.LastOrDefault()));
            }
        }

        private void TeethViewModelOnToothChanged(object sender, ToothChangedEventArgs e)
        {
            ToothChanged?.Invoke(this, e);

            OnPropertyChanged(() => Description);
            OnPropertyChanged(() => IsValid);
        }

        private bool CanSelectProstheticConditionCommandHandler()
        {
            return SelectedTeeth != null;
        }

        private void SelectProstheticConditionsCommandHandler(object obj)
        {
            var prostheticConditionViewModel = (CheckableDictionaryItemViewModel)obj;

            foreach (var selectedTooth in SelectedTeeth)
            {
                selectedTooth.ProstheticConditions.First(x => x.DisplayName == prostheticConditionViewModel.DisplayName).IsChecked = true;
            }
        }

        private bool CanSelectTypeOfProstheticsCommandHandler()
        {
            return SelectedTeeth != null;
        }

        private void SelectProstheticsTypeCommandHandler(object obj)
        {
            var prostheticTypeViewModel = (CheckableDictionaryItemViewModel)obj;

            foreach (var selectedTooth in SelectedTeeth)
            {
                selectedTooth.ProstheticTypes.First(x => x.DisplayName == prostheticTypeViewModel.DisplayName).IsChecked = true;
            }
        }

        private bool CanSelectMaterialCommandHandler()
        {
            return SelectedTeeth != null;
        }

        private void SelectMaterialCommandHandler(object obj)
        {
            var materialViewModel = (CheckableDictionaryItemViewModel)obj;

            foreach (var selectedTooth in SelectedTeeth)
            {
                selectedTooth.Materials.First(x => x.DisplayName == materialViewModel.DisplayName).IsChecked = true;
            }
        }

        private bool CanSelectBridgeCommandHandler()
        {
            return SelectedTeeth != null;
        }

        private void SelectBridgeCommandHandler(object obj)
        {
            var toothViewModel = obj as ToothViewModel;
            if (toothViewModel != null)
            {
                foreach (var selectedTooth in SelectedTeeth)
                {
                    selectedTooth.HasBridge = toothViewModel.HasBridge;
                }
            }
            else
            {
                var hasBridge = SelectedTeeth.First().HasBridge;
                foreach (var selectedTooth in SelectedTeeth)
                {
                    selectedTooth.HasBridge = !hasBridge;
                }
            }
        }

        private bool CanClearCommandHandler()
        {
            return SelectedTeeth != null && SelectedTeeth.Any(x => x.CanClear);
        }

        private void ClearCommandHandler()
        {
            SelectedTeeth.ForEach(x => x.Clear());
        }

        private bool CanCopyDescriptionCommandHandler()
        {
            return !string.IsNullOrWhiteSpace(Description);
        }

        private void CopyDescriptionCommandHandler()
        {
            _clipboard.CopyToClipboard(Description);
        }

        public void AttachPricePositions(PricePositionViewModel[] pricePositions)
        {
            if (SelectedTeeth == null || !SelectedTeeth.Any())
                return;

            foreach (var toothViewModel in SelectedTeeth)
            {
                toothViewModel.AttachPricePositions(pricePositions);
            }

            //SelectedTeeth.Last().AttachPricePositions(pricePositions);
        }
    }
}