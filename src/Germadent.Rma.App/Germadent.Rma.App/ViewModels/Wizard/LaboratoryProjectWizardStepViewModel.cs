using System.Collections.ObjectModel;
using System.Linq;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.Model;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public interface IMouthProvider
    {
        ObservableCollection<TeethViewModel> Mouth { get; }
    }

    public class LaboratoryProjectWizardStepViewModel : ViewModelBase, IWizardStepViewModel, IMouthProvider
    {
        private IRmaOperations _rmaOperations;

        private string _workDescription;
        private string _colorAndFeatures;
        private int _transparency;

        public LaboratoryProjectWizardStepViewModel(IRmaOperations rmaOperations)
        {
            _rmaOperations = rmaOperations;
        }

        public string DisplayName
        {
            get { return "Проект"; }
        }

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

        public void Initialize(OrderDto order)
        {
            FillTeethCollection(order);

            WorkDescription = order.WorkDescription;
            ColorAndFeatures = order.ColorAndFeatures;
            Transparency = order.Transparency;

            OnPropertyChanged();
        }

        public void AssemblyOrder(OrderDto order)
        {
            order.WorkDescription = WorkDescription;
            order.ColorAndFeatures = ColorAndFeatures;
            order.Transparency = order.Transparency;

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
    }
}