using System;
using Germadent.Rma.Model;
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

        private DateTime? _fittingDate;
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

        public DateTime? FittingDate
        {
            get { return _fittingDate; }
            set { SetProperty(() => _fittingDate, value); }
        }

        public DateTime? WorkCompleted
        {
            get { return _workCompleted; }
            set { SetProperty(() => _workCompleted, value); }
        }

        public void Initialize(Order order)
        {
            Customer = order.Customer;
            DoctorFio = order.Doctor;
            Sex = order.Sex;
            Age = order.Age;
            PatientFio = order.Patient;
            Created = order.Created;
            FittingDate = order.FittingDate;
            WorkCompleted = order.Closed;
        }

        public void AssemblyOrder(Order order)
        {
            order.BranchType = BranchType.Laboratory;
            order.Customer = Customer;
            order.Doctor = DoctorFio;
            order.Sex = Sex;
            order.Age = Age;
            order.Patient = PatientFio;
            order.Created = Created;
            order.FittingDate = FittingDate;
            order.Closed = WorkCompleted;
        }
    }
}