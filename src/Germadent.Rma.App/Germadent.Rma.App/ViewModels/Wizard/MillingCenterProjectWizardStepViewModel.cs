using System.Collections.ObjectModel;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.Model;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public class MillingCenterProjectWizardStepViewModel : WizardStepViewModelBase, IMouthProvider
    {
        private readonly IRmaOperations _rmaOperations;

        private string _workDescription;
        private string _carcassColor;
        private string _additionalMillingInfo;
        private string _individualAbutmentProcessing;
        private string _understaff;
        private bool _workAccepted;

        public MillingCenterProjectWizardStepViewModel(IRmaOperations rmaOperations)
        {
            _rmaOperations = rmaOperations;
        }

        public override string DisplayName
        {
            get { return "Проект"; }
        }

        public string AdditionalMillingInfo
        {
            get { return _additionalMillingInfo; }
            set { SetProperty(() => _additionalMillingInfo, value); }
        }

        public string WorkDescription
        {
            get { return _workDescription; }
            set { SetProperty(() => _workDescription, value); }
        }

        public string CarcassColor
        {
            get { return _carcassColor; }
            set { SetProperty(() => _carcassColor, value); }
        }

       
        public string IndividualAbutmentProcessing
        {
            get { return _individualAbutmentProcessing; }
            set { SetProperty(() => _individualAbutmentProcessing, value); }
        }

        public string Understaff
        {
            get { return _understaff; }
            set { SetProperty(() => _understaff, value); }
        }

        public bool WorkAccepted
        {
            get { return _workAccepted; }
            set { SetProperty(() => _workAccepted, value); }
        }

        public ObservableCollection<TeethViewModel> Mouth { get; } = new ObservableCollection<TeethViewModel>();

        public override void Initialize(OrderDto order)
        {
            AdditionalMillingInfo = order.AdditionalInfo;
            WorkDescription = order.WorkDescription;
            CarcassColor = order.CarcassColor;
            IndividualAbutmentProcessing = order.IndividualAbutmentProcessing;
            Understaff = order.Understaff;
            WorkAccepted = order.WorkAccepted;

            FillTeethCollection();

            OnPropertyChanged();
        }

        public override void AssemblyOrder(OrderDto order)
        {
            order.AdditionalInfo = AdditionalMillingInfo;
            order.WorkDescription = WorkDescription;
            order.CarcassColor = CarcassColor;
            order.IndividualAbutmentProcessing = IndividualAbutmentProcessing;
            order.Understaff = Understaff;
            order.WorkAccepted = WorkAccepted;
        }

        private void FillTeethCollection()
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
        }
    }
}