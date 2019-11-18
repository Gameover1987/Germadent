using System;
using System.Collections.ObjectModel;
using Germadent.Rma.Model;
using Germadent.Rma.Model.Operation;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels
{
    public class MillingCenterOrderViewModel : ViewModelBase, IMillingOrderViewModel
    {
        private readonly IRmaOperations _rmaOperations;

        private string _customer;
        private string _patientFio;
        private string _technicFio;
        private string _technicPhone;
        private DateTime _workReceived;
        private AdditionalMillingInfo _additionalMillingInfo;
        private CarcasColor _carcasColor;

        private string _implantSystemDescription;

        // Обработка инд. абатмента
        private bool _machining;
        private bool _polishing;
        private bool _ledgePlunge05;
        private bool _ledgePlunge10;
        private int _ledgePlungeCustom;

        // Докомплектовать заказ
        private string _screw;
        private string _titaniumBase;

        // Описание работ
        private string _workDescription;

        private bool _agreement;

        public MillingCenterOrderViewModel(IRmaOperations rmaOperations)
        {
            _rmaOperations = rmaOperations;
        }

        public bool IsReadOnly { get; set; }

        public string Customer
        {
            get { return _customer; }
            set { SetProperty(() => _customer, value); }
        }

        public string TechnicFio
        {
            get { return _technicFio; }
            set { SetProperty(() => _technicFio, value); }
        }

        public string PatientFio
        {
            get { return _patientFio; }
            set { SetProperty(() => _patientFio, value); }
        }

        public string TechnicPhoneNumber
        {
            get { return _technicPhone; }
            set { SetProperty(() => _technicPhone, value); }
        }

        public AdditionalMillingInfo AdditionalMillingInfo
        {
            get { return _additionalMillingInfo; }
            set { SetProperty(() => _additionalMillingInfo, value); }
        }

        public CarcasColor CarcasColor
        {
            get { return _carcasColor; }
            set { SetProperty(() => _carcasColor, value); }
        }

        public string ImplantSystemDescription
        {
            get { return _implantSystemDescription; }
            set { SetProperty(() => _implantSystemDescription, value); }
        }

        public bool Matching
        {
            get { return _machining; }
            set { SetProperty(() => _machining, value); }
        }

        public bool Polishing
        {
            get { return _polishing; }
            set { SetProperty(() => _polishing, value); }
        }

        public bool LeedgePlunge05
        {
            get { return _ledgePlunge05; }
            set { SetProperty(() => _ledgePlunge05, value); }
        }

        public bool LeedgePlunge10
        {
            get { return _ledgePlunge10; }
            set { SetProperty(() => _ledgePlunge10, value); }
        }

        public int LedgePlungeCustom
        {
            get { return _ledgePlungeCustom; }
            set { SetProperty(() => _ledgePlungeCustom, value); }
        }

        public DateTime WorkReceived
        {
            get { return _workReceived; }
            set { SetProperty(() => _workReceived, value); }
        }

        public string Screw
        {
            get { return _screw; }
            set { SetProperty(() => _screw, value); }
        }

        public string TitaniumBase
        {
            get { return _titaniumBase; }
            set { SetProperty(() => _titaniumBase, value); }
        }

        public string WorkDescription
        {
            get { return _workDescription; }
            set { SetProperty(() => _workDescription, value); }
        }

        public bool Agreement
        {
            get { return _agreement; }
            set { SetProperty(() => _agreement, value); }
        }

        public ObservableCollection<MaterialViewModel> Materials { get; } = new ObservableCollection<MaterialViewModel>();

        public ObservableCollection<TeethViewModel> Mouth { get; } = new ObservableCollection<TeethViewModel>();


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