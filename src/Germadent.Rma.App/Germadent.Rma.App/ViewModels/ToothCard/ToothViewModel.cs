using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Germadent.Common.Extensions;
using Germadent.Rma.Model;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.ToothCard
{
    public class ToothChangedEventArgs
    {
        public ToothChangedEventArgs(bool affectsRenderToothCard)
        {
            AffectsRenderToothCard = affectsRenderToothCard;
        }

        public bool AffectsRenderToothCard { get; }
    }

    public class ToothViewModel : ViewModelBase
    {
        private bool _hasBridge;
        private bool _isChecked;

        public ToothViewModel(MaterialDto[] materials, ProstheticsTypeDto[] prostheticTypes)
        {
            materials.ForEach(x =>
            {
                var materialViewModel = new MaterialViewModel(x);
                materialViewModel.Checked += MaterialViewModelOnChecked;
                Materials.Add(materialViewModel);
            });
            prostheticTypes.ForEach(x =>
            {
                var prostheticsTypeViewModel = new ProstheticsTypeViewModel(x);
                prostheticsTypeViewModel.Checked += ProstheticsTypeViewModelOnChecked;
                ProstheticTypes.Add(prostheticsTypeViewModel);
            });
        }

        public int Number { get; set; }

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                //if (_isChecked == value)
                //    return;
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

        public ObservableCollection<MaterialViewModel> Materials { get; } = new ObservableCollection<MaterialViewModel>();

        public MaterialViewModel SelectedMaterial
        {
            get { return Materials.FirstOrDefault(x => x.IsChecked); }
        }

        public ObservableCollection<ProstheticsTypeViewModel> ProstheticTypes { get; } = new ObservableCollection<ProstheticsTypeViewModel>();


        public ProstheticsTypeViewModel SelectedProstheticsType
        {
            get { return ProstheticTypes.FirstOrDefault(x => x.IsChecked); }
        }

        public bool HasDescription
        {
            get { return SelectedProstheticsType != null || SelectedMaterial != null || HasBridge; }
        }

        public string Description
        {
            get
            {
                var descriptionBuilder = new StringBuilder();
                if (SelectedProstheticsType != null)
                    descriptionBuilder.Append(string.Format("{0}/", SelectedProstheticsType.DisplayName));

                if (SelectedMaterial != null)
                    descriptionBuilder.Append(string.Format("{0}/", SelectedMaterial.DisplayName));

                if (HasBridge)
                    descriptionBuilder.Append("Мост");

                return descriptionBuilder.ToString().Trim(new[] { ' ', '/' });
            }
        }

        public event EventHandler<ToothChangedEventArgs> ToothChanged;

        public void Initialize(ToothDto toothDto)
        {
            IsChecked = true;
            Number = toothDto.ToothNumber;

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
                MaterialName = SelectedMaterial?.Item.Name,
                ProstheticsId = SelectedProstheticsType?.Item.Id ?? 0,
                ProstheticsName = SelectedProstheticsType?.Item.Name,
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
            Materials.ForEach(x => x.ResetIsChecked());
            ProstheticTypes.ForEach(x => x.ResetIsChecked());

            IsChecked = false;
        }

        public override string ToString()
        {
            return Description;
        }

        private bool GetIsChecked()
        {
            return HasBridge || SelectedProstheticsType != null || SelectedMaterial != null;
        }

        private void ProstheticsTypeViewModelOnChecked(object sender, EventArgs e)
        {
            var prostheticsTypeViewModels = ProstheticTypes.Where(x => x != sender);
            prostheticsTypeViewModels.ForEach(x => x.ResetIsChecked());

            OnPropertyChanged(() => SelectedProstheticsType);
            OnPropertyChanged(() => HasDescription);
            OnPropertyChanged(() => Description);
            IsChecked = GetIsChecked();
        }

        private void MaterialViewModelOnChecked(object sender, EventArgs e)
        {
            var materialViewModels = Materials.Where(x => x != sender);
            materialViewModels.ForEach(x => x.ResetIsChecked());

            OnPropertyChanged(() => SelectedMaterial);
            OnPropertyChanged(() => HasDescription);
            OnPropertyChanged(() => Description);
            IsChecked = GetIsChecked();
        }
    }
}
