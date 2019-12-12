﻿using System;
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
        private bool _nonOpacity;
        private bool _highOpacity;
        private bool _lowOpacity;
        private bool _mamelons;
        private bool _secondaryDentin;
        private bool _paintedFissurs;


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

        public ObservableCollection<MaterialViewModel> Materials { get; } = new ObservableCollection<MaterialViewModel>();

        public ObservableCollection<TeethViewModel> Mouth { get; } = new ObservableCollection<TeethViewModel>();

        public void Initialize(Order order)
        {
            FillTeethCollection(order);

            WorkDescription = order.WorkDescription;
            ColorAndFeatures = order.ColorAndFeatures;
            NonOpacity = order.NonOpacity;
            HighOpacity = order.HighOpacity;
            LowOpacity = order.LowOpacity;
            Mamelons = order.Mamelons;
            SecondaryDentin = order.SecondaryDentin;
            PaintedFissurs = order.PaintedFissurs;

            OnPropertyChanged();
        }

        public void AssemblyOrder(Order order)
        {
            order.WorkDescription = WorkDescription;
            order.ColorAndFeatures = ColorAndFeatures;
            order.NonOpacity = NonOpacity;
            order.HighOpacity = HighOpacity;
            order.LowOpacity = LowOpacity;
            order.Mamelons = Mamelons;
            order.SecondaryDentin = SecondaryDentin;
            order.PaintedFissurs = PaintedFissurs;
            order.Mouth = Mouth.Where(x => x.IsChecked).Select(x => x.ToModel()).ToArray();
        }

        private void FillTeethCollection(Order order)
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