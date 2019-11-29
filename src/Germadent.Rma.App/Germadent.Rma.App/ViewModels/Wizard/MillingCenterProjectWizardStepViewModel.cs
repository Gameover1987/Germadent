﻿using System;
using System.Collections.ObjectModel;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.Model;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public class MillingCenterProjectWizardStepViewModel : ViewModelBase, IWizardStepViewModel, IMouthProvider
    {
        private readonly IRmaOperations _rmaOperations;

        private string _workDescription;
        private string _color;

        private bool _nonOpacity;
        private bool _highOpacity;
        private bool _lowOpacity;
        private bool _mamelons;
        private bool _secondaryDentin;
        private bool _paintedFissurs;
        private AdditionalMillingInfo _additionalMillingInfo;
        private int _ledgePlungeCustom;
        private string _screw;
        private string _titaniumBase;
        private bool _agreement;

        public MillingCenterProjectWizardStepViewModel(IRmaOperations rmaOperations)
        {
            _rmaOperations = rmaOperations;
        }

        public string DisplayName
        {
            get { return "Проект"; }
        }

        public AdditionalMillingInfo AdditionalMillingInfo
        {
            get { return _additionalMillingInfo; }
            set { SetProperty(() => _additionalMillingInfo, value); }
        }

        public string WorkDescription
        {
            get { return _workDescription; }
            set { SetProperty(() => _workDescription, value); }
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

        public int LedgePlungeCustom
        {
            get { return _ledgePlungeCustom; }
            set { SetProperty(() => _ledgePlungeCustom, value); }
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

        public bool Agreement
        {
            get { return _agreement; }
            set { SetProperty(() => _agreement, value); }
        }

        public ObservableCollection<MaterialViewModel> Materials { get; } = new ObservableCollection<MaterialViewModel>();

        public ObservableCollection<TeethViewModel> Mouth { get; } = new ObservableCollection<TeethViewModel>();

        public void Initialize(Order order)
        {
            FillMaterials();

            FillTeethCollection();

            OnPropertyChanged();
        }

        public void AssemblyOrder(Order order)
        {
            
        }

        private void FillMaterials()
        {
            var materials = _rmaOperations.GetMaterials();
            foreach (var material in materials)
            {
                Materials.Add(new MaterialViewModel(material));
            }
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