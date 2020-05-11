﻿using System;
using System.Linq;
using Germadent.Common.Extensions;
using Germadent.Rma.App.Infrastructure;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;
using Germadent.Rma.Model;
using Germadent.UI.Commands;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public class MillingCenterInfoWizardStepViewModel : WizardStepViewModelBase
    {
        private readonly ICatalogSelectionOperations _catalogSelectionOperations;
        private readonly ICatalogUIOperations _catalogUIOperations;
        private readonly IResponsiblePersonsSuggestionsProvider _responsiblePersonsSuggestionsProvider;
        private readonly ICustomerRepository _customerRepository;
        private readonly IResponsiblePersonRepository _responsiblePersonRepository;

        private int _customerId;
        private int _responsiblePersonId;
        private string _customer;
        private string _patient;
        private string _responsiblePerson;
        private string _responsiblePersonPhone;
        private DateTime _created;
        private string _dateComment;

        public MillingCenterInfoWizardStepViewModel(ICatalogSelectionOperations catalogSelectionOperations,
            ICatalogUIOperations catalogUIOperations,
            ICustomerSuggestionProvider customerSuggestionProvider,
            IResponsiblePersonsSuggestionsProvider responsiblePersonsSuggestionsProvider,
            ICustomerRepository customerRepository,
            IResponsiblePersonRepository responsiblePersonRepository)
        {
            _catalogSelectionOperations = catalogSelectionOperations;
            _catalogUIOperations = catalogUIOperations;
            _responsiblePersonsSuggestionsProvider = responsiblePersonsSuggestionsProvider;
            _customerRepository = customerRepository;
            _responsiblePersonRepository = responsiblePersonRepository;
            _customerRepository.Changed += CustomerRepositoryOnChanged;
            CustomerSuggestionProvider = customerSuggestionProvider;
            ResponsiblePersonsSuggestionsProvider = responsiblePersonsSuggestionsProvider;

            AddValidationFor(() => Customer)
                .When(() => Customer.IsNullOrWhiteSpace(), () => "Укажите заказчика");

            SelectCustomerCommand = new DelegateCommand(SelectCustomerCommandHandler);
            AddCustomerCommand = new DelegateCommand(AddCustomerCommandHandler, CanAddCustomerCommandHandler);
            ShowCustomerCardCommand = new DelegateCommand(ShowCustomerCardCommandHandler, CanShowCustomerCardCommandHandler);

            SelectResponsiblePersonCommand = new DelegateCommand(SelectResponsiblePersonCommandHandler);
            AddResponsiblePersonCommand = new DelegateCommand(AddResponsiblePersonCommandHandler, CanAddResponsiblePersonCommandHandler);
            ShowResponsiblePersonCardCommand = new DelegateCommand(ShowResponsiblePersonCardCommandHandler, CanShowResponsiblePersonCardCommandHandler);
        }

        public override bool IsValid => !HasErrors && !Customer.IsNullOrWhiteSpace() && !IsNewCustomer;

        public override string DisplayName => "Общие данные";

        public string Customer
        {
            get => _customer;
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

        public ICustomerSuggestionProvider CustomerSuggestionProvider { get; }

        public string Patient
        {
            get => _patient;
            set
            {
                if (_patient == value)
                    return;
                _patient = value;
                OnPropertyChanged(() => Patient);
            }
        }

        public string ResponsiblePerson
        {
            get => _responsiblePerson;
            set
            {
                if (_responsiblePerson == value)
                    return;
                _responsiblePerson = value;
                OnPropertyChanged(() => ResponsiblePerson);
            }
        }

        public bool IsNewResponsiblePerson
        {
            get
            {
                if (ResponsiblePerson.IsNullOrWhiteSpace())
                    return false;

                return _responsiblePersonRepository.Items.All(x => x.FullName != Customer);
            }
        }

        public IResponsiblePersonsSuggestionsProvider ResponsiblePersonsSuggestionsProvider { get; }

        public string ResponsiblePersonPhone
        {
            get => _responsiblePersonPhone;
            set
            {
                if (_responsiblePersonPhone == value)
                    return;
                _responsiblePersonPhone = value;
                OnPropertyChanged(() => ResponsiblePersonPhone);
            }
        }

        public DateTime Created
        {
            get => _created;
            set
            {
                if (_created == value)
                    return;
                _created = value;
                OnPropertyChanged(() => Created);
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

        public IDelegateCommand SelectCustomerCommand { get; }

        public IDelegateCommand AddCustomerCommand { get; }

        public IDelegateCommand ShowCustomerCardCommand { get; }

        public IDelegateCommand SelectResponsiblePersonCommand { get; }

        public IDelegateCommand AddResponsiblePersonCommand { get; }

        public IDelegateCommand ShowResponsiblePersonCardCommand { get; }

        public override void Initialize(OrderDto order)
        {
            _customerId = order.CustomerId;
            _customer = order.Customer;
            _patient = order.Patient;
            _responsiblePersonId = order.ResponsiblePersonId;
            _responsiblePerson = order.ResponsiblePerson;
            _responsiblePersonPhone = order.ResponsiblePersonPhone;
            _created = order.Created;
            _dateComment = order.DateComment;
        }

        public override void AssemblyOrder(OrderDto order)
        {
            order.CustomerId = _customerId;
            order.Customer = Customer;
            order.Patient = Patient;
            order.ResponsiblePersonId = _responsiblePersonId;
            order.ResponsiblePerson = ResponsiblePerson;
            order.ResponsiblePersonPhone = ResponsiblePersonPhone;
            order.Created = Created;
            order.DateComment = DateComment;
        }

        private void CustomerRepositoryOnChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(() => IsNewCustomer);
        }

        private void SelectCustomerCommandHandler()
        {
            var customer = _catalogSelectionOperations.SelectCustomer(Customer);
            if (customer == null)
                return;

            _customerId = customer.Id;
            Customer = customer.Name;
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

        private bool CanShowCustomerCardCommandHandler()
        {
            if (Customer.IsNullOrWhiteSpace())
                return false;

            return _customerRepository.Items.Any(x => x.Name == Customer);
        }

        private void ShowCustomerCardCommandHandler()
        {
            _catalogSelectionOperations.ShowCustomerCart(_customerRepository.Items.First(x => x.Name == Customer));
        }

        private void SelectResponsiblePersonCommandHandler()
        {
            var responsiblePerson = _catalogSelectionOperations.SelectResponsiblePerson(ResponsiblePerson);
            if (responsiblePerson == null)
                return;

            _responsiblePersonId = responsiblePerson.Id;
            ResponsiblePerson = responsiblePerson.FullName;
        }

        private bool CanAddResponsiblePersonCommandHandler()
        {
            if (ResponsiblePerson.IsNullOrWhiteSpace())
                return false;

            return _responsiblePersonRepository.Items.All(x => x.FullName != ResponsiblePerson);
        }

        private void AddResponsiblePersonCommandHandler()
        {

        }

        private bool CanShowResponsiblePersonCardCommandHandler()
        {
            if (ResponsiblePerson.IsNullOrWhiteSpace())
                return false;

            return _responsiblePersonRepository.Items.Any(x => x.FullName == ResponsiblePerson);
        }

        private void ShowResponsiblePersonCardCommandHandler()
        {

        }
    }
}
