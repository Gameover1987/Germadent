using System;
using Germadent.Rma.Model;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public class MillingCenterInfoWizardStepViewModel : WizardStepViewModelBase
    {
        private string _customer;
        private string _patient;
        private string _responsiblePerson;
        private string _responsiblePersonPhone;
        private DateTime _created;

        public MillingCenterInfoWizardStepViewModel()
        {
            
        }

        public override bool IsValid => !HasErrors;

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

        public string ResponsiblePerson
        {
            get { return _responsiblePerson; }
            set { SetProperty(() => _responsiblePerson, value); }
        }

        public string ResponsiblePersonPhone
        {
            get { return _responsiblePersonPhone; }
            set { SetProperty(() => _responsiblePersonPhone, value); }
        }

        public DateTime Created
        {
            get { return _created; }
            set { SetProperty(() => _created, value); }
        }

        public override void Initialize(OrderDto order)
        {
            Customer = order.Customer;
            Patient = order.Patient;
            ResponsiblePerson = order.ResponsiblePerson;
            ResponsiblePersonPhone = order.ResponsiblePersonPhone;
            Created = order.Created;
        }

        public override void AssemblyOrder(OrderDto order)
        {
            order.Customer = Customer;
            order.Patient = Patient;
            order.ResponsiblePerson = ResponsiblePerson;
            order.ResponsiblePersonPhone = ResponsiblePersonPhone;
            order.Created = Created;
        }
    }
}
