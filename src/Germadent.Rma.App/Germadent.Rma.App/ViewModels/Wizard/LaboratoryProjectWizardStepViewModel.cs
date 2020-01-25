using System.Collections.ObjectModel;
using System.Linq;
using Germadent.Common.Extensions;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.Model;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public interface IMouthProvider
    {
        ObservableCollection<TeethViewModel> Mouth { get; }

        ObservableCollection<MaterialViewModel> Materials { get; }

        ObservableCollection<ProstheticsTypeViewModel> ProstheticTypes { get; }
    }

    public class LaboratoryProjectWizardStepViewModel : WizardStepViewModelBase, IMouthProvider
    {
        private readonly IRmaOperations _rmaOperations;

        private string _workDescription;
        private string _colorAndFeatures;
        private int _transparency;

        public LaboratoryProjectWizardStepViewModel(IRmaOperations rmaOperations)
        {
            _rmaOperations = rmaOperations;
        }

        public override string DisplayName
        {
            get { return "Проект"; }
        }

        public override bool IsValid => !HasErrors;

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

        public int Transparency
        {
            get { return _transparency; }
            set { SetProperty(() => _transparency, value); }
        }

        public ObservableCollection<TeethViewModel> Mouth { get; } = new ObservableCollection<TeethViewModel>();
        public ObservableCollection<MaterialViewModel> Materials { get; } = new ObservableCollection<MaterialViewModel>();
        public ObservableCollection<ProstheticsTypeViewModel> ProstheticTypes { get; } = new ObservableCollection<ProstheticsTypeViewModel>();

        public override void Initialize(OrderDto order)
        {
            FillTeethCollection(order);
            FillMaterialCollection();
            FillProstheticTypesCollection();

            WorkDescription = order.WorkDescription;
            ColorAndFeatures = order.ColorAndFeatures;
            Transparency = order.Transparency;

            OnPropertyChanged();
        }

        public override void AssemblyOrder(OrderDto order)
        {
            order.WorkDescription = WorkDescription;
            order.ColorAndFeatures = ColorAndFeatures;
            order.Transparency = Transparency;

            order.Mouth = Mouth.Where(x => x.IsChecked).Select(x => x.ToModel()).ToArray();
        }

        private void FillTeethCollection(OrderDto order)
        {
            Mouth.Clear();
            for (int i = 21; i <= 28; i++)
            {
                Mouth.Add(new TeethViewModel { Number = i });
            }

            for (int i = 31; i <= 38; i++)
            {
                Mouth.Add(new TeethViewModel { Number = i });
            }

            for (int i = 41; i <= 48; i++)
            {
                Mouth.Add(new TeethViewModel { Number = i });
            }

            for (int i = 11; i <= 18; i++)
            {
                Mouth.Add(new TeethViewModel { Number = i });
            }

            if (order.Mouth != null)
            {
                foreach (var teethFromOrder in order.Mouth)
                {
                    var teeth = Mouth.Single(x => x.Number == teethFromOrder.Number);
                    teeth.IsChecked = true;
                    teeth.HasBridge = teethFromOrder.HasBridge;
                }
            }
        }

        private void FillMaterialCollection()
        {
            Materials.Clear();
            _rmaOperations.GetMaterials().ForEach(x => Materials.Add(new MaterialViewModel(x)));
        }

        private void FillProstheticTypesCollection()
        {
            ProstheticTypes.Clear();
            _rmaOperations.GetProstheticTypes().ForEach(x => ProstheticTypes.Add(new ProstheticsTypeViewModel(x)));
        }
    }
}