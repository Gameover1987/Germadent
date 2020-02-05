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
        private bool _isChecked;

        public ToothViewModel(ProstheticConditionDto[] prostheticConditions, ProstheticsTypeDto[] prostheticTypes, MaterialDto[] materials)
        {
            prostheticConditions.ForEach(x =>
            {
                var prostheticConditionViewModel = new ProstheticConditionViewModel(x);
                prostheticConditionViewModel.Checked += ProstheticConditionViewModelOnChecked;
                ProstheticConditions.Add(prostheticConditionViewModel);
            });
            prostheticTypes.ForEach(x =>
            {
                var prostheticsTypeViewModel = new ProstheticsTypeViewModel(x);
                prostheticsTypeViewModel.Checked += ProstheticsTypeViewModelOnChecked;
                ProstheticTypes.Add(prostheticsTypeViewModel);
            });
            materials.ForEach(x =>
            {
                var materialViewModel = new MaterialViewModel(x);
                materialViewModel.Checked += MaterialViewModelOnChecked;
                Materials.Add(materialViewModel);
            });
        }

        public int Number { get; set; }

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnPropertyChanged(() => IsChecked);

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
                OnPropertyChanged(() => HasBridge);
                OnPropertyChanged(() => Description);

                _isChecked = GetIsChecked();
                OnPropertyChanged(() => IsChecked);

                ToothChanged?.Invoke(this, new ToothChangedEventArgs(true));
            }
        }

        public ObservableCollection<ProstheticConditionViewModel> ProstheticConditions { get; } = new ObservableCollection<ProstheticConditionViewModel>();

        public ProstheticConditionViewModel SelectedProstheticCondition
        {
            get { return ProstheticConditions.FirstOrDefault(x => x.IsChecked); }
        }

        public ObservableCollection<ProstheticsTypeViewModel> ProstheticTypes { get; } = new ObservableCollection<ProstheticsTypeViewModel>();

        public ProstheticsTypeViewModel SelectedProstheticsType
        {
            get { return ProstheticTypes.FirstOrDefault(x => x.IsChecked); }
        }

        public ObservableCollection<MaterialViewModel> Materials { get; } = new ObservableCollection<MaterialViewModel>();


        public MaterialViewModel SelectedMaterial
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
                if (!IsChecked)
                    return true;

                return SelectedProstheticCondition != null && SelectedMaterial != null && SelectedProstheticsType != null;
            }
        }

        public string Description
        {
            get
            {
                var descriptionBuilder = new StringBuilder();

                if (SelectedProstheticCondition != null)
                    descriptionBuilder.Append(string.Format("{0}/", SelectedProstheticCondition.DisplayName));

                if (SelectedProstheticsType != null)
                    descriptionBuilder.Append(string.Format("{0}/", SelectedProstheticsType.DisplayName));

                if (SelectedMaterial != null)
                    descriptionBuilder.Append(string.Format("{0}/", SelectedMaterial.DisplayName));

                if (HasBridge)
                    descriptionBuilder.Append("Мост");

                return descriptionBuilder.ToString().Trim(new[] { ' ', '/' });
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
            IsChecked = true;
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
                ConditionId = SelectedProstheticCondition.Item.Id,
                ConditionName = SelectedProstheticCondition.DisplayName,
                HasBridge = HasBridge
            };
        }

        public bool CanClear
        {
            get { return HasBridge || SelectedMaterial != null || SelectedProstheticsType != null; }
        }

        public void Clear()
        {
            HasBridge = false;
            ProstheticConditions.ForEach(x => x.ResetIsChanged());
            Materials.ForEach(x => x.ResetIsChanged());
            ProstheticTypes.ForEach(x => x.ResetIsChanged());

            IsChecked = false;
        }

        public override string ToString()
        {
            return Description;
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
            OnPropertyChanged(() => HasDescription);
            OnPropertyChanged(() => Description);
            IsChecked = GetIsChecked();
        }

        private void ProstheticsTypeViewModelOnChecked(object sender, EventArgs e)
        {
            var prostheticsTypeViewModels = ProstheticTypes.Where(x => x != sender);
            prostheticsTypeViewModels.ForEach(x => x.ResetIsChanged());

            OnPropertyChanged(() => SelectedProstheticsType);
            OnPropertyChanged(() => HasDescription);
            OnPropertyChanged(() => Description);
            IsChecked = GetIsChecked();
        }

        private void MaterialViewModelOnChecked(object sender, EventArgs e)
        {
            var materialViewModels = Materials.Where(x => x != sender);
            materialViewModels.ForEach(x => x.ResetIsChanged());

            OnPropertyChanged(() => SelectedMaterial);
            OnPropertyChanged(() => HasDescription);
            OnPropertyChanged(() => Description);
            IsChecked = GetIsChecked();
        }
    }
}
