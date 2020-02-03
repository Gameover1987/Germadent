using System.Collections.ObjectModel;
using System.Linq;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public class ToothCardViewModel : IToothCardViewModel
    {
        private readonly IRmaOperations _rmaOperations;

        public ToothCardViewModel(IRmaOperations rmaOperations)
        {
            _rmaOperations = rmaOperations;

            Teeth = new ObservableCollection<ToothViewModel>();
            Materials = new ObservableCollection<MaterialViewModel>();
            Prosthetics = new ObservableCollection<ProstheticsTypeViewModel>();
        }

        public ObservableCollection<ToothViewModel> Teeth { get; }

        public ObservableCollection<MaterialViewModel> Materials { get; }

        public ObservableCollection<ProstheticsTypeViewModel> Prosthetics { get; }

        public void Initialize(ToothDto[] toothCard)
        {
            var materials = _rmaOperations.GetMaterials();
            var prosteticTypes = _rmaOperations.GetProstheticTypes();

            Teeth.Clear();
            for (int i = 21; i <= 28; i++)
            {
                Teeth.Add(new ToothViewModel(materials, prosteticTypes) { Number = i });
            }

            for (int i = 31; i <= 38; i++)
            {
                Teeth.Add(new ToothViewModel(materials, prosteticTypes) { Number = i });
            }

            for (int i = 41; i <= 48; i++)
            {
                Teeth.Add(new ToothViewModel(materials, prosteticTypes) { Number = i });
            }

            for (int i = 11; i <= 18; i++)
            {
                Teeth.Add(new ToothViewModel(materials, prosteticTypes) { Number = i });
            }

            foreach (var teethViewModel in Teeth)
            {
                var toothDto = toothCard.FirstOrDefault(x => x.ToothNumber == teethViewModel.Number);
                if (toothDto == null)
                    continue;

                teethViewModel.Initialize(toothDto);
            }
        }

        public ToothDto[] ToModel()
        {
            return Teeth.Where(x => x.IsChecked).Select(x => x.ToModel()).ToArray();
        }
    }
}