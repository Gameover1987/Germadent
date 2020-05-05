using System;
using System.Linq;
using Germadent.Common.Extensions;
using Germadent.Rma.App.Infrastructure;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.Model;
using Germadent.UI.Commands;
using Germadent.UI.Controls;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public class MillingCenterInfoWizardStepViewModel : WizardStepViewModelBase
    {
        private readonly ICatalogUIOperations _catalogUIOperations;
        private readonly ICustomerRepository _customerRepository;

        private int _customerId;
        private string _customer;
        private string _patient;
        private string _technicFullName;
        private string _technicPhone;
        private DateTime _created;
        private string _dateComment;

        public MillingCenterInfoWizardStepViewModel(ICatalogUIOperations catalogUIOperations, ISuggestionProvider customerSuggestionProvider, ICustomerRepository customerRepository)
        {
            _catalogUIOperations = catalogUIOperations;
            _customerRepository = customerRepository;
            _customerRepository.Changed += CustomerRepositoryOnChanged;
            CustomerSuggestionProvider = customerSuggestionProvider;

            AddValidationFor(() => Customer)
                .When(() => Customer.IsNullOrWhiteSpace(), () => "Укажите заказчика");

            SelectCustomerCommand = new DelegateCommand(SelectCustomerCommandHandler);
            AddCustomerCommand = new DelegateCommand(AddCustomerCommandHandler, CanAddCustomerCommandHandler);
            ShowCustomerCartCommand = new DelegateCommand(ShowCustomerCartCommandHandler, CanShowCustomerCartCommandHandler);
        }

        public override bool IsValid => !HasErrors && !Customer.IsNullOrWhiteSpace() && !IsNewCustomer;

        public override string DisplayName
        {
            get { return "Общие данные"; }
        }

        public string Customer
        {
            get { return _customer; }
            set
            {
                if (_customer == value)
                    return;
                _customer = value;
                OnPropertyChanged(() => Customer);
                OnPropertyChanged(() => IsNewCustomer);

                if (!IsNewCustomer && !Customer.IsNullOrWhiteSpace())
                {
                    _customerId = _customerRepository.Items.First(x => x.Name == Customer).Id;
                }
            }
        }

        public bool IsNewCustomer
        {
            get
            {
                if (Customer.IsNullOrWhiteSpace())
                    return false;

                return _customerRepository.Items.All(x => x.Name != Customer);
            }
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

        public IDelegateCommand AddCustomerCommand { get; }

        public IDelegateCommand ShowCustomerCartCommand { get; }

        public override void Initialize(OrderDto order)
        {
            _customerId = order.CustomerId;
            _customer = order.Customer;
            _patient = order.Patient;
            _technicFullName = order.ResponsiblePerson;
            _technicPhone = order.ResponsiblePersonPhone;
            _created = order.Created;
            _dateComment = order.DateComment;
        }

        public override void AssemblyOrder(OrderDto order)
        {
            order.CustomerId = _customerId;
            order.Customer = Customer;
            order.Patient = Patient;
            order.ResponsiblePerson = TechnicFullName;
            order.ResponsiblePersonPhone = TechnicPhone;
            order.Created = Created;
            order.DateComment = DateComment;
        }

        private void CustomerRepositoryOnChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(() => IsNewCustomer);
        }

        private void SelectCustomerCommandHandler()
        {
            _catalogUIOperations.Initialize();
            var customer = _catalogUIOperations.SelectCustomer(Customer);
            if (customer == null)
                return;

            _customerId = customer.CustomerId;
            Customer = customer.DisplayName;
        }

        private bool CanAddCustomerCommandHandler()
        {
            if (Customer.IsNullOrWhiteSpace())
                return false;

            return _customerRepository.Items.All(x => x.Name != Customer);
        }

        private void AddCustomerCommandHandler()
        {
            var customer = _catalogUIOperations.AddCustomer(new CustomerDto { Name = Customer });
            if (customer == null)
                return;

            _customerId = customer.Id;
        }


        private bool CanShowCustomerCartCommandHandler()
        {
            if (Customer.IsNullOrWhiteSpace())
                return false;

            return _customerRepository.Items.Any(x => x.Name == Customer);
        }

        private void ShowCustomerCartCommandHandler()
        {
            _catalogUIOperations.ShowCustomerCart(_customerRepository.Items.First(x => x.Name == Customer));
        }
    }
}
