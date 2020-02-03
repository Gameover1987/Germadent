using System.Collections.ObjectModel;
using System.Linq;
using Germadent.Common.Extensions;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.ViewModels
{
    public class ToothViewModel
    {
        public ToothViewModel(MaterialDto[] materials, ProstheticsTypeDto[] prostheticTypes)
        {
            materials.ForEach(x => Materials.Add(new MaterialViewModel(x)));
            prostheticTypes.ForEach(x => ProstheticTypes.Add(new ProstheticsTypeViewModel(x)));
        }

        public int Number { get; set; }

        public bool IsChecked { get; set; }

        public bool HasBridge { get; set; }

        public ObservableCollection<MaterialViewModel> Materials { get; } = new ObservableCollection<MaterialViewModel>();

        public ObservableCollection<ProstheticsTypeViewModel> ProstheticTypes { get; } = new ObservableCollection<ProstheticsTypeViewModel>();

        public void Initialize(ToothDto toothDto)
        {
            IsChecked = true;
            HasBridge = toothDto.HasBridge;

            var selectedMaterial = Materials.FirstOrDefault(x => x.Item.Name == toothDto.MaterialName);
            if (selectedMaterial != null)
                selectedMaterial.IsChecked = true;

            var selectedProstheticsType = ProstheticTypes.FirstOrDefault(x => x.Item.Name == toothDto.ProstheticsName);
            if (selectedProstheticsType != null)
                selectedProstheticsType.IsChecked = true;
        }

        public ToothDto ToModel()
        {
            var selectedMaterial = Materials.FirstOrDefault(x => x.IsChecked);
            var selectedProstheticsType = ProstheticTypes.FirstOrDefault(x => x.IsChecked);
            return new ToothDto()
            {
                ToothNumber = Number,
                MaterialId = selectedMaterial?.Item.Id ?? 0,
                MaterialName = selectedMaterial?.Item.Name,
                ProstheticsId = selectedProstheticsType?.Item.Id ?? 0,
                ProstheticsName = selectedProstheticsType?.Item.Name,
                HasBridge = HasBridge
            };
        }

        public override string ToString()
        {
            return string.Format("Number={0}, IsChecked={1}, HasBridge={2}", Number, IsChecked, HasBridge);
        }
    }
}
