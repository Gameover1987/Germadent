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

        public override string DisplayName => "Общие данные";

        public override bool IsValid => !HasErrors && !IsEmpty();

        public string Customer
        {
            get => _customer;
            set
            {
                if (_customer == value)
                    return;
                _customer = value;
                OnPropertyChanged(() => Customer);
            }
        }

        public string DoctorFio
        {
            get => _doctorFio;
            set
            {
                if (_doctorFio == value)
                    return;
                _doctorFio = value;
                OnPropertyChanged(() => DoctorFio);
            }
        }

        public Gender Gender
        {
            get => _gender;
            set {
                if (_gender == value)
                    return;
                _gender = value;
                OnPropertyChanged(() => Gender);
            }
        }

        public int Age
        {
            get => _age;
            set
            {
                if (_age == value)
                    return;
                _age = value;
                OnPropertyChanged(() => Age);
            }
        }

        public string PatientFio
        {
            get => _patientFio;
            set {
                if (_patientFio == value)
                    return;
                _patientFio = value;
                OnPropertyChanged(() => PatientFio);
            }
        }

        public DateTime Created
        {
            get => _created;
            set {
                if (_created == value)
                    return;
                _created = value;
                OnPropertyChanged(() => Created);
            }
        }

        public DateTime? FittingDate
        {
            get => _fittingDate;
            set {
                if (_fittingDate == value)
                    return;
                _fittingDate = value;
                OnPropertyChanged(() => FittingDate);
            }
        }

        public DateTime? DateOfCompletion
        {
            get => _dateOfCompletion;
            set {
                if (_dateOfCompletion == value)
                    return;
                _dateOfCompletion = value;
                OnPropertyChanged(() => DateOfCompletion);
            }
        }

        public string DateComment
        {
            get => _dateComment;
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