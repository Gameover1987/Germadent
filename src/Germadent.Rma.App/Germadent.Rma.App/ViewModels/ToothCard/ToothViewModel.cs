using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Germadent.Common.Extensions;
using Germadent.Rma.Model;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.ToothCard
{
    public class ToothViewModel : ViewModelBase
    {
        private bool _hasBridge;
        private bool _isChanged;

        public ToothViewModel(DictionaryItemDto[] prostheticConditions, DictionaryItemDto[] prostheticTypes, DictionaryItemDto[] materials)
        {
            prostheticConditions.ForEach(x =>
            {
                var prostheticConditionViewModel = new CheckableDictionaryItemViewModel(x);
                prostheticConditionViewModel.Checked += ProstheticConditionViewModelOnChecked;
                ProstheticConditions.Add(prostheticConditionViewModel);
            });
            prostheticTypes.ForEach(x =>
            {
                var prostheticsTypeViewModel = new CheckableDictionaryItemViewModel(x);
                prostheticsTypeViewModel.Checked += ProstheticsTypeViewModelOnChecked;
                ProstheticTypes.Add(prostheticsTypeViewModel);
            });
            materials.ForEach(x =>
            {
                var materialViewModel = new CheckableDictionaryItemViewModel(x);
                materialViewModel.Checked += MaterialViewModelOnChecked;
                Materials.Add(materialViewModel);
            });
        }

        public int Number { get; set; }

        public bool IsChanged
        {
            get { return _isChanged; }
            set
            {
                _isChanged = value;
                OnPropertyChanged(() => IsChanged);

                ToothChanged?.Invoke(this, new ToothChangedEventArgs(false));
            }
        }

        public bool HasBridge
        {
            get { return _hasBridge; }
            set
            {
                if (_hasBridge == value)
                    return;

                _hasBridge = value;
                _isChanged = GetIsChecked();
                OnPropertyChanged(() => HasBridge);
                OnPropertyChanged(() => IsChanged);

                ToothChanged?.Invoke(this, new ToothChangedEventArgs(true));
            }
        }

        public ObservableCollection<CheckableDictionaryItemViewModel> ProstheticConditions { get; } = new ObservableCollection<CheckableDictionaryItemViewModel>();

        public CheckableDictionaryItemViewModel SelectedProstheticCondition
        {
            get { return ProstheticConditions.FirstOrDefault(x => x.IsChecked); }
        }

        public ObservableCollection<CheckableDictionaryItemViewModel> ProstheticTypes { get; } = new ObservableCollection<CheckableDictionaryItemViewModel>();

        public CheckableDictionaryItemViewModel SelectedProstheticsType
        {
            get { return ProstheticTypes.FirstOrDefault(x => x.IsChecked); }
        }

        public ObservableCollection<CheckableDictionaryItemViewModel> Materials { get; } = new ObservableCollection<CheckableDictionaryItemViewModel>();


        public CheckableDictionaryItemViewModel SelectedMaterial
        {
            get { return Materials.FirstOrDefault(x => x.IsChecked); }
        }

        public bool HasDescription
        {
            get { return SelectedProstheticsType != null || SelectedMaterial != null || HasBridge; }
        }

        public bool IsValid
        {
            get
            {
                if (!IsChanged)
                    return true;

                return SelectedProstheticCondition != null && SelectedMaterial != null && SelectedProstheticsType != null;
            }
        }

        public string Description
        {
            get
            {
                var dto = ToDto();
                return OrderDescriptionBuilder.GetToothCardDescription(dto);
            }
        }

        public string ErrorDescription
        {
            get
            {
                var descriptionBuilder = new StringBuilder();
                if (SelectedProstheticCondition == null)
                    descriptionBuilder.AppendLine("Выберите условие протезирования");

                if (SelectedProstheticsType == null)
                    descriptionBuilder.AppendLine("Выьерите тип протезирования");

                if (SelectedMaterial == null)
                    descriptionBuilder.AppendLine("Выберите материал");

                return descriptionBuilder.ToString();
            }
        }

        public event EventHandler<ToothChangedEventArgs> ToothChanged;

        public void Initialize(ToothDto toothDto)
        {
            IsChanged = true;
            Number = toothDto.ToothNumber;

            var selectedProstheticCondition = ProstheticConditions.FirstOrDefault(x => x.DisplayName == toothDto.ConditionName);
            if (selectedProstheticCondition != null)
                selectedProstheticCondition.IsChecked = true;

            var selectedProstheticsType = ProstheticTypes.FirstOrDefault(x => x.Item.Name == toothDto.ProstheticsName);
            if (selectedProstheticsType != null)
                selectedProstheticsType.IsChecked = true;

            var selectedMaterial = Materials.FirstOrDefault(x => x.Item.Name == toothDto.MaterialName);
            if (selectedMaterial != null)
                selectedMaterial.IsChecked = true;

            _hasBridge = toothDto.HasBridge;

            OnPropertyChanged();
        }

        public ToothDto ToDto()
        {
            return new ToothDto()
            {
                ToothNumber = Number,
                MaterialId = SelectedMaterial?.Item.Id ?? 0,
                MaterialName = SelectedMaterial?.DisplayName,
                ProstheticsId = SelectedProstheticsType?.Item.Id ?? 0,
                ProstheticsName = SelectedProstheticsType?.DisplayName,
                ConditionId = SelectedProstheticCondition?.Item.Id ?? 0,
                ConditionName = SelectedProstheticCondition?.DisplayName,
                HasBridge = HasBridge
            };
        }

        public bool CanClear
        {
            get { return HasBridge || SelectedProstheticCondition != null || SelectedMaterial != null || SelectedProstheticsType != null; }
        }

        public void Clear()
        {
            HasBridge = false;
            ProstheticConditions.ForEach(x => x.ResetIsChanged());
            Materials.ForEach(x => x.ResetIsChanged());
            ProstheticTypes.ForEach(x => x.ResetIsChanged());

            IsChanged = false;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Number, Description); ;
        }

        private bool GetIsChecked()
        {
            return HasBridge || SelectedProstheticCondition != null || SelectedProstheticsType != null || SelectedMaterial != null;
        }

        private void ProstheticConditionViewModelOnChecked(object sender, EventArgs e)
        {
            var prostheticConditionViewModels = ProstheticConditions.Where(x => x != sender);
            prostheticConditionViewModels.ForEach(x => x.ResetIsChanged());

            OnPropertyChanged(() => SelectedProstheticCondition);
            IsChanged = GetIsChecked();
        }

        private void ProstheticsTypeViewModelOnChecked(object sender, EventArgs e)
        {
            var prostheticsTypeViewModels = ProstheticTypes.Where(x => x != sender);
            prostheticsTypeViewModels.ForEach(x => x.ResetIsChanged());

            OnPropertyChanged(() => SelectedProstheticsType);
            IsChanged = GetIsChecked();
        }

        private void MaterialViewModelOnChecked(object sender, EventArgs e)
        {
            var materialViewModels = Materials.Where(x => x != sender);
            materialViewModels.ForEach(x => x.ResetIsChanged());

            OnPropertyChanged(() => SelectedMaterial);
            IsChanged = GetIsChecked();
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            base.OnPropertyChanged(nameof(IsValid));
            base.OnPropertyChanged(nameof(ErrorDescription));
            base.OnPropertyChanged(nameof(HasDescription));
            base.OnPropertyChanged(nameof(Description));
        }
    }
}
