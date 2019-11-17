using System;
using System.Collections.ObjectModel;
using Germadent.ServiceClient.Model;
using Germadent.ServiceClient.Operation;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels
{
    public interface ILabOrderViewModel : IOrderViewModel
    { }

    public class LabOrderViewModel : ViewModelBase, ILabOrderViewModel
    {
        private readonly IRmaOperations _rmaOperations;

        private string _customer;
        private string _doctorFio;
        private string _patientFio;
        private DateTime _created;
        private string _workDescription;
        private DateTime? _workStarted;
        private DateTime? _workCompleted;
        private string _color;

        private bool _nonOpacity;
        private bool _highOpacity;
        private bool _lowOpacity;
        private bool _mamelons;
        private bool _secondaryDentin;
        private bool _paintedFissurs;
        private Sex _sex;
        private int _age;

        public LabOrderViewModel(IRmaOperations rmaOperations)
        {
            _rmaOperations = rmaOperations;
        }

        public bool IsReadOnly { get; private set; }

        public string Customer
        {
            get { return _customer; }
            set { SetProperty(() => _customer, value); }
        }

        public string DoctorFio
        {
            get { return _doctorFio; }
            set { SetProperty(() => _doctorFio, value); }
        }

        public string PatientFio
        {
            get { return _patientFio; }
            set { SetProperty(() => _patientFio, value); }
        }

        public DateTime Created
        {
            get { return _created; }
            set { SetProperty(() => _created, value); }
        }

        public string WorkDescription
        {
            get { return _workDescription; }
            set { SetProperty(() => _workDescription, value); }
        }

        public DateTime? WorkStarted
        {
            get { return _workStarted; }
            set { SetProperty(() => _workStarted, value); }
        }

        public DateTime? WorkCompleted
        {
            get { return _workCompleted; }
            set { SetProperty(() => _workCompleted, value); }
        }

        public ObservableCollection<MaterialViewModel> Materials { get; } = new ObservableCollection<MaterialViewModel>();

        public ObservableCollection<TeethViewModel> Mouth { get; } = new ObservableCollection<TeethViewModel>();

        public Sex Sex
        {
            get { return _sex; }
            set { SetProperty(() => _sex, value); }
        }

        public int Age
        {
            get { return _age; }
            set { SetProperty(() => _age, value); }
        }

        public string Color
        {
            get { return _color; }
            set { SetProperty(() => _color, value); }
        }

        public bool NonOpacity
        {
            get { return _nonOpacity; }
            set { SetProperty(() => _nonOpacity, value); }
        }

        public bool HighOpacity
        {
            get { return _highOpacity; }
            set { SetProperty(() => _highOpacity, value); }
        }

        public bool LowOpacity
        {
            get { return _lowOpacity; }
            set { SetProperty(() => _lowOpacity, value); }
        }

        public bool Mamelons
        {
            get { return _mamelons; }
            set { SetProperty(() => _mamelons, value); }
        }

        public bool SecondaryDentin
        {
            get { return _secondaryDentin; }
            set { SetProperty(() => _secondaryDentin, value); }
        }

        public bool PaintedFissurs
        {
            get { return _paintedFissurs; }
            set { SetProperty(() => _paintedFissurs, value); }
        }

        public void Initialize(bool isReadOnly)
        {
            IsReadOnly = isReadOnly;

            var materials = _rmaOperations.GetMaterials();
            foreach (var material in materials)
            {
                Materials.Add(new MaterialViewModel(material));
            }

            FillTeethCollection();

            OnPropertyChanged();
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