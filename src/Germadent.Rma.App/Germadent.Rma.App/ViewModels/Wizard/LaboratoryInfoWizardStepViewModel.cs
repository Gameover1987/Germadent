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

        private Gender _gender;
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

        public Gender Gender
        {
            get { return _gender; }
            set { SetProperty(() => _gender, value); }
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

        public void Initialize(OrderDto order)
        {
            Customer = order.Customer;
            DoctorFio = order.ResponsiblePerson;
            Gender = order.Gender;
            Age = order.Age;
            PatientFio = order.Patient;
            Created = order.Created;
            FittingDate = order.FittingDate;
            WorkCompleted = order.Closed;
        }

        public void AssemblyOrder(OrderDto order)
        {
            order.BranchType = BranchType.Laboratory;
            order.Customer = Customer;
            order.ResponsiblePerson = DoctorFio;
            order.Gender = Gender;
            order.Age = Age;
            order.Patient = PatientFio;
            order.Created = Created;
            order.FittingDate = FittingDate;
            order.Closed = WorkCompleted;
        }
    }
}