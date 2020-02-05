using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Germadent.Common.CopyAndPaste;
using Germadent.Common.Extensions;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.Model;
using Germadent.UI.Commands;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.ToothCard
{
    public class ToothCardViewModel : ViewModelBase, IToothCardViewModel
    {
        private readonly IRmaOperations _rmaOperations;
        private readonly IClipboardHelper _clipboard;
        private ToothViewModel[] _selectedTeeth;

        public ToothCardViewModel(IRmaOperations rmaOperations, IClipboardHelper clipboard)
        {
            _rmaOperations = rmaOperations;
            _clipboard = clipboard;

            Teeth = new ObservableCollection<ToothViewModel>();
            Materials = new ObservableCollection<MaterialViewModel>();
            Prosthetics = new ObservableCollection<ProstheticsTypeViewModel>();

            SelectPtostheticsConditionCommand = new DelegateCommand(x => SelectProstheticConditionsCommandHandler(x), x => CanSelectProstheticConditionCommandHandler());
            SelectPtostheticsTypeCommand = new DelegateCommand(x => SelectProstheticsTypeCommandHandler(x), x => CanSelectTypeOfProstheticsCommandHandler());
            SelectMaterialCommand = new DelegateCommand(x => SelectMaterialCommandHandler(x), x => CanSelectMaterialCommandHandler());
            SelectBridgeCommand = new DelegateCommand(x => SelectBridgeCommandHandler(x), x => CanSelectBridgeCommandHandler());
            ClearCommand = new DelegateCommand(x => ClearCommandHandler(), x => CanClearCommandHandler());
            CopyDescriptionCommand = new DelegateCommand(x => CopyDescriptionCommandHandler(), x => CanCopyDescriptionCommandHandler());
        }

        public ObservableCollection<ToothViewModel> Teeth { get; }

        public ObservableCollection<MaterialViewModel> Materials { get; }

        public ObservableCollection<ProstheticsTypeViewModel> Prosthetics { get; }

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

        public void Initialize(ToothDto[] toothCard)
        {
            var prostheticConditions = _rmaOperations.GetProstheticConditions();
            var materials = _rmaOperations.GetMaterials();
            var prosteticTypes = _rmaOperations.GetProstheticTypes();

            foreach (var toothViewModel in Teeth)
            {
                toothViewModel.ToothChanged -= TeethViewModelOnToothChanged;
            }
            Teeth.Clear();
            for (int i = 21; i <= 28; i++)
            {
                Teeth.Add(new ToothViewModel(prostheticConditions, prosteticTypes, materials) { Number = i });
            }

            for (int i = 31; i <= 38; i++)
            {
                Teeth.Add(new ToothViewModel(prostheticConditions, prosteticTypes, materials) { Number = i });
            }

            for (int i = 41; i <= 48; i++)
            {
                Teeth.Add(new ToothViewModel(prostheticConditions,prosteticTypes, materials) { Number = i });
            }

            for (int i = 11; i <= 18; i++)
            {
                Teeth.Add(new ToothViewModel(prostheticConditions, prosteticTypes, materials) { Number = i });
            }

            foreach (var teethViewModel in Teeth)
            {
                teethViewModel.ToothChanged += TeethViewModelOnToothChanged;

                var toothDto = toothCard.FirstOrDefault(x => x.ToothNumber == teethViewModel.Number);
                if (toothDto == null)
                    continue;

                teethViewModel.Initialize(toothDto);
            }
        }

        public ToothDto[] ToDto()
        {
            return Teeth.Where(x => x.IsChanged).Select(x => x.ToDto()).ToArray();
        }

        public event EventHandler<EventArgs> RenderRequest;

        public string Description
        {
            get
            {
                var builder = new StringBuilder();

                foreach (var toothViewModel in Teeth.Where(x => x.IsChanged).OrderBy(x => x.Number))
                {
                    builder.Append(string.Format("{0} - {1}, ", toothViewModel.Number, toothViewModel.Description));
                }

                return builder.ToString().Trim(new[] { ' ', ',' });
            }
        }

        public ToothViewModel[] SelectedTeeth
        {
            get { return _selectedTeeth; }
            set
            {
                _selectedTeeth = value;
                OnPropertyChanged(() => SelectedTeeth);
            }
        }

        private void TeethViewModelOnToothChanged(object sender, ToothChangedEventArgs e)
        {
            if (e.AffectsRenderToothCard)
                RenderRequest?.Invoke(this, EventArgs.Empty);

            OnPropertyChanged(() => Description);
            OnPropertyChanged(() => IsValid);
        }

        private bool CanSelectProstheticConditionCommandHandler()
        {
            return SelectedTeeth != null;
        }

        private void SelectProstheticConditionsCommandHandler(object obj)
        {
            var prostheticConditionViewModel = (ProstheticConditionViewModel)obj;

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
            var prostheticTypeViewModel = (ProstheticsTypeViewModel)obj;

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
            var materialViewModel = (MaterialViewModel)obj;

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
            var toothViewModel = (ToothViewModel)obj;

            foreach (var selectedTooth in SelectedTeeth)
            {
                selectedTooth.HasBridge = toothViewModel.HasBridge;
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
    }
}