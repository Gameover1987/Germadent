using System;
using Germadent.Common.Extensions;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public class MillingCenterInfoWizardStepViewModel : WizardStepViewModelBase
    {
        private string _customer;
        private string _patient;
        private string _technicFullName;
        private string _technicPhone;
        private DateTime _created;

        public MillingCenterInfoWizardStepViewModel()
        {
            AddValidationFor(() => Customer)
                .When(() => Customer.IsNullOrWhiteSpace(), () => "Укажите заказчика");
            AddValidationFor(() => Patient)
                .When(() => Patient.IsNullOrWhiteSpace(), () => "Укажите пациента");
        }

        public override bool IsValid => !HasErrors && !IsEmpty();

        public override string DisplayName
        {
            get { return "Общие данные"; }
        }

        public string Customer
        {
            get { return _customer; }
            set { SetProperty(() => _customer, value); }
        }

        public string Patient
        {
            get { return _patient; }
            set { SetProperty(() => _patient, value); }
        }

        public string TechnicFullName
        {
            get { return _technicFullName; }
            set { SetProperty(() => _technicFullName, value); }
        }

        public string TechnicPhone
        {
            get { return _technicPhone; }
            set { SetProperty(() => _technicPhone, value); }
        }

        public DateTime Created
        {
            get { return _created; }
            set { SetProperty(() => _created, value); }
        }

        public override void Initialize(OrderDto order)
        {
            _customer = order.Customer;
            _patient = order.Patient;
            _technicFullName = order.ResponsiblePerson;
            _technicPhone = order.ResponsiblePersonPhone;
            _created = order.Created;
        }

        public override void AssemblyOrder(OrderDto order)
        {
            order.Customer = Customer;
            order.Patient = Patient;
            order.ResponsiblePerson = TechnicFullName;
            order.ResponsiblePersonPhone = TechnicPhone;
            order.Created = Created;
        }

        private bool IsEmpty()
        {
            return Customer.IsNullOrWhiteSpace() ||
                   Patient.IsNullOrWhiteSpace();
        }
    }
}
