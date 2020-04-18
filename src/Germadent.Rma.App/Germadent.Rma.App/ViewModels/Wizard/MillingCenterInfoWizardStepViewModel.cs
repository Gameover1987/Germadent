using System;
using Germadent.Common.Extensions;
using Germadent.Rma.App.Infrastructure;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;
using Germadent.Rma.App.Views;
using Germadent.Rma.Model;
using Germadent.UI.Commands;
using Germadent.UI.Controls;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public class MillingCenterInfoWizardStepViewModel : WizardStepViewModelBase
    {
        private readonly ICatalogUIOperations _catalogUIOperations;

        private string _customer;
        private string _patient;
        private string _technicFullName;
        private string _technicPhone;
        private DateTime _created;
        private string _dateComment;

        public MillingCenterInfoWizardStepViewModel(ICatalogUIOperations catalogUIOperations, ISuggestionProvider customerSuggestionProvider)
        {
            _catalogUIOperations = catalogUIOperations;
            CustomerSuggestionProvider = customerSuggestionProvider;

            AddValidationFor(() => Customer)
                .When(() => Customer.IsNullOrWhiteSpace(), () => "Укажите заказчика");

            SelectCustomerCommand = new DelegateCommand(SelectCustomerCommandHandler);
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

        public ISuggestionProvider CustomerSuggestionProvider { get; }

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

        public IDelegateCommand SelectCustomerCommand { get; }

        public override void Initialize(OrderDto order)
        {
            _customer = order.Customer;
            _patient = order.Patient;
            _technicFullName = order.ResponsiblePerson;
            _technicPhone = order.ResponsiblePersonPhone;
            _created = order.Created;
            _dateComment = order.DateComment;
        }

        public override void AssemblyOrder(OrderDto order)
        {
            order.Customer = Customer;
            order.Patient = Patient;
            order.ResponsiblePerson = TechnicFullName;
            order.ResponsiblePersonPhone = TechnicPhone;
            order.Created = Created;
            order.DateComment = DateComment;
        }

        private void SelectCustomerCommandHandler()
        {
            var customer = _catalogUIOperations.SelectCustomer(Customer);
            if (customer == null)
                return;

            Customer = customer.DisplayName;
        }

        private bool IsEmpty()
        {
            return Customer.IsNullOrWhiteSpace() ||
                   Patient.IsNullOrWhiteSpace();
        }
    }
}
