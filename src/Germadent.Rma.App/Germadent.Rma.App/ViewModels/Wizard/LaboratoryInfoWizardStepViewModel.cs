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
            var labOrder = (LaboratoryOrder)order;

            Customer = labOrder.Customer;
            DoctorFio = labOrder.Doctor;
            Sex = labOrder.Sex;
            Age = labOrder.Age;
            PatientFio = labOrder.Patient;
            Created = labOrder.Created;
            FittingDate = labOrder.FittingDate;
            WorkCompleted = labOrder.Closed;
        }

        public void AssemblyOrder(Order order)
        {
            var labOrder = (LaboratoryOrder) order;

            labOrder.BranchType = BranchType.Laboratory;
            labOrder.Customer = Customer;
            labOrder.Doctor = DoctorFio;
            labOrder.Sex = Sex;
            labOrder.Age = Age;
            labOrder.Patient = PatientFio;
            labOrder.Created = Created;
            labOrder.FittingDate = FittingDate;
            labOrder.Closed = WorkCompleted;
        }
    }
}