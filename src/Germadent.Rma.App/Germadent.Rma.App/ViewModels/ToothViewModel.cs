using System;
using System.Collections.ObjectModel;
using System.Linq;
using Germadent.Common.Extensions;
using Germadent.Rma.Model;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels
{
    public class ToothViewModel : ViewModelBase
    {
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

        public bool IsChecked { get; set; }

        public bool HasBridge { get; set; }

        public ObservableCollection<MaterialViewModel> Materials { get; } = new ObservableCollection<MaterialViewModel>();

        public MaterialViewModel SelectedMaterial { get; set; }

        public ObservableCollection<ProstheticsTypeViewModel> ProstheticTypes { get; } = new ObservableCollection<ProstheticsTypeViewModel>();

        public ProstheticsTypeViewModel SelectedProstheticsType { get; set; }

        public void Initialize(ToothDto toothDto)
        {
            IsChecked = true;
            Number = toothDto.ToothNumber;
            HasBridge = toothDto.HasBridge;
            SelectedMaterial = Materials.FirstOrDefault(x => x.Item.Name == toothDto.MaterialName);
            SelectedProstheticsType = ProstheticTypes.FirstOrDefault(x => x.Item.Name == toothDto.ProstheticsName);
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

        public override string ToString()
        {
            return string.Format("Number={0}, IsChecked={1}, HasBridge={2}", Number, IsChecked, HasBridge);
        }

        private void ProstheticsTypeViewModelOnChecked(object sender, EventArgs e)
        {
            var prostheticsTypeViewModels = ProstheticTypes.Where(x => x != sender);
            prostheticsTypeViewModels.ForEach(x => x.ResetIsChecked());

            SelectedProstheticsType = (ProstheticsTypeViewModel)sender;
            IsChecked = true;
        }

        private void MaterialViewModelOnChecked(object sender, EventArgs e)
        {
            var materialViewModels = Materials.Where(x => x != sender);
            materialViewModels.ForEach(x => x.ResetIsChecked());

            SelectedMaterial = (MaterialViewModel)sender;
            IsChecked = true;
        }
    }
}
