using System;
using Germadent.Common.Extensions;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public class LaboratoryInfoWizardStepViewModel : WizardStepViewModelBase
    {
        private string _customer;
        private string _doctorFio;
        private string _patientFio;

        private Gender _gender;
        private int _age;

        private DateTime _created;

        private DateTime? _fittingDate;
        private DateTime? _dateOfCompletion;

        private string _dateComment;

        public LaboratoryInfoWizardStepViewModel()
        {
            AddValidationFor(() => Customer)
                .When(() => Customer.IsNullOrWhiteSpace(), () => "Укажите заказчика");
            AddValidationFor(() => DoctorFio)
                .When(() => DoctorFio.IsNullOrWhiteSpace(), () => "Укажите фамилию доктора");
            AddValidationFor(() => PatientFio)
                .When(() => PatientFio.IsNullOrWhiteSpace(), () => "Укажите фамилию пациента");
            AddValidationFor(() => Age)
                .When(() => Age < 0 || Age > 150, () => "Возраст должен быть в диапазоне от 0 до 150");
        }

        public override string DisplayName
        {
            get { return "Общие данные"; }
        }

        public override bool IsValid
        {
            get { return !HasErrors && !IsEmpty(); }
        }

        public string Customer
        {
            get { return _customer; }
            set
            {
                SetProperty(() => _customer, value); 
            }
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

        public DateTime? DateOfCompletion
        {
            get { return _dateOfCompletion; }
            set { SetProperty(() => _dateOfCompletion, value); }
        }

        public string DateComment
        {
            get { return _dateComment; }
            set
            {
                if (_dateComment == value)
                    return;
                _dateComment = value;
                OnPropertyChanged(() => DateComment);
            }
        }

        public override void Initialize(OrderDto order)
        {
            Customer = order.Customer;
            DoctorFio = order.ResponsiblePerson;
            Gender = order.Gender;
            Age = order.Age;
            PatientFio = order.Patient;
            Created = order.Created;
            FittingDate = order.FittingDate;
            DateOfCompletion = order.DateOfCompletion;
            DateComment = order.DateComment;
        }

        public override void AssemblyOrder(OrderDto order)
        {
            order.BranchType = BranchType.Laboratory;
            order.Customer = Customer;
            order.ResponsiblePerson = DoctorFio;
            order.Gender = Gender;
            order.Age = Age;
            order.Patient = PatientFio;
            order.Created = Created;
            order.FittingDate = FittingDate;
            order.DateOfCompletion = DateOfCompletion;
            order.DateComment = DateComment;
        }

        private bool IsEmpty()
        {
            return Customer.IsNullOrWhiteSpace() ||
                   PatientFio.IsNullOrWhiteSpace() ||
                   DoctorFio.IsNullOrWhiteSpace();
        }
    }
}