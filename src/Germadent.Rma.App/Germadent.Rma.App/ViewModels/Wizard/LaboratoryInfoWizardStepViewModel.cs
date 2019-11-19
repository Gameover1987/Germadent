using System;
using Germadent.Rma.Model;
using Germadent.Rma.Model.Operation;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public class LaboratoryInfoWizardStepViewModel : ViewModelBase, IWizardStepViewModel
    {
        private string _customer;
        private string _doctorFio;
        private string _patientFio;

        private Sex _sex;
        private int _age;

        private DateTime _created;

        private DateTime? _workStarted;
        private DateTime? _workCompleted;

        public LaboratoryInfoWizardStepViewModel()
        {
            
        }

        public string DisplayName
        {
            get { return "Общие данные"; }
        }

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

        public void Initialize(Order order)
        {
            
        }
    }
}