﻿using System;
using System.Linq;
using DocumentFormat.OpenXml.Spreadsheet;
using Germadent.Client.Common.ServiceClient;
using Germadent.Client.Common.ServiceClient.Repository;
using Germadent.Common.Extensions;
using Germadent.Model;
using Germadent.Rma.App.Operations;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;
using Germadent.UI.Commands;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public class LaboratoryInfoWizardStepViewModel : WizardStepViewModelBase
    {
        private readonly ICatalogSelectionUIOperations _catalogSelectionOperations;
        private readonly ICatalogUIOperations _catalogUIOperations;
        private readonly ICustomerRepository _customerRepository;
        private readonly IResponsiblePersonRepository _responsiblePersonRepository;

        private string _customer;
        private string _responsiblePerson;
        private string _patientFio;
        private Gender _gender;
        private int _age;
        private DateTime _created;
        private DateTime? _fittingDate;
        private DateTime? _dateOfCompletion;
        private string _dateComment;
        private int _customerId;
        private int _responsiblePersonId;
        private bool _stl;
        private bool _cashless;
        private float _urgencyRatio;
        private bool _isNormalUrgencyRatio;
        private bool _isHighUrgencyRatio;

        public LaboratoryInfoWizardStepViewModel(ICatalogSelectionUIOperations catalogSelectionOperations,
            ICatalogUIOperations catalogUIOperations,
            ICustomerSuggestionProvider customerSuggestionProvider,
            IResponsiblePersonsSuggestionsProvider responsiblePersonsSuggestionsProvider,
            ICustomerRepository customerRepository,
            IResponsiblePersonRepository responsiblePersonRepository)
        {
            _catalogSelectionOperations = catalogSelectionOperations;
            _catalogUIOperations = catalogUIOperations;
            _customerRepository = customerRepository;
            _customerRepository.Changed += CustomerRepositoryOnChanged;
            _responsiblePersonRepository = responsiblePersonRepository;
            _responsiblePersonRepository.Changed += ResponsiblePersonRepositoryOnChanged;

            CustomerSuggestionProvider = customerSuggestionProvider;
            ResponsiblePersonsSuggestionsProvider = responsiblePersonsSuggestionsProvider;

            AddValidationFor(() => Customer)
                .When(() => Customer.IsNullOrWhiteSpace(), () => "Укажите заказчика");
            AddValidationFor(() => ResponsiblePerson)
                .When(() => ResponsiblePerson.IsNullOrWhiteSpace(), () => "Укажите фамилию доктора");
            AddValidationFor(() => Age)
                .When(() => Age < 0 || Age > 150, () => "Возраст должен быть в диапазоне от 0 до 150");

            SelectCustomerCommand = new DelegateCommand(SelectCustomerCommandHandler);
            AddCustomerCommand = new DelegateCommand(AddCustomerCommandHandler, CanAddCustomerCommandHandler);
            ShowCustomerCardCommand = new DelegateCommand(ShowCustomerCardCommandHandler, CanShowCustomerCardCommandHandler);

            SelectResponsiblePersonCommand = new DelegateCommand(SelectResponsiblePersonCommandHandler);
            AddResponsiblePersonCommand = new DelegateCommand(AddResponsiblePersonCommandHandler, CanAddResponsiblePersonCommandHandler);
            ShowResponsiblePersonCardCommand = new DelegateCommand(ShowResponsiblePersonCardCommandHandler, CanShowResponsiblePersonCardCommandHandler);
        }

        public override string DisplayName => "Общие данные";

        public override bool IsValid
        {
            get
            {
                if (HasErrors)
                    return false;

                if (Customer.IsNullOrWhiteSpace())
                    return false;

                if (IsNewCustomer)
                    return false;

                if (ResponsiblePerson.IsNullOrWhiteSpace())
                    return false;

                if (IsNewResponsiblePerson)
                    return false;

                return true;
            }
        }

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

        public string ResponsiblePerson
        {
            get => _responsiblePerson;
            set
            {
                if (_responsiblePerson == value)
                    return;
                _responsiblePerson = value;
                OnPropertyChanged(() => ResponsiblePerson);
                OnPropertyChanged(() => IsNewResponsiblePerson);

                if (!IsNewResponsiblePerson && !ResponsiblePerson.IsNullOrWhiteSpace())
                {
                    _responsiblePersonId = _responsiblePersonRepository.Items.First(x => x.FullName == ResponsiblePerson).Id;
                }
            }
        }

        public bool IsNewResponsiblePerson
        {
            get
            {
                if (ResponsiblePerson.IsNullOrWhiteSpace())
                    return false;

                return _responsiblePersonRepository.Items.All(x => x.FullName != ResponsiblePerson);
            }
        }

        public IResponsiblePersonsSuggestionsProvider ResponsiblePersonsSuggestionsProvider { get; }

        public Gender Gender
        {
            get => _gender;
            set
            {
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
            set
            {
                if (_patientFio == value)
                    return;
                _patientFio = value;
                OnPropertyChanged(() => PatientFio);
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

        public DateTime? FittingDate
        {
            get => _fittingDate;
            set
            {
                if (_fittingDate == value)
                    return;
                _fittingDate = value;
                OnPropertyChanged(() => FittingDate);
            }
        }

        public DateTime? DateOfCompletion
        {
            get => _dateOfCompletion;
            set
            {
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

        public bool Stl
        {
            get { return _stl; }
            set
            {
                if (_stl == value)
                    return;
                _stl = value;
                OnPropertyChanged(() => Stl);
            }
        }

        public bool Cashless
        {
            get { return _cashless; }
            set
            {
                if (_cashless == value)
                    return;
                _cashless = value;
                OnPropertyChanged(() => Cashless);
            }
        }

        public bool IsNormalUrgencyRatio
        {
            get { return _isNormalUrgencyRatio; }
            set
            {
                _isNormalUrgencyRatio = value;
                OnPropertyChanged(() => IsNormalUrgencyRatio);

                if (IsNormalUrgencyRatio)
                    UrgencyRatio = OrderDto.NormalUrgencyRatio;
            }
        }

        public bool IsHighUrgencyRatio
        {
            get { return _isHighUrgencyRatio; }
            set
            {
                _isHighUrgencyRatio = value;
                OnPropertyChanged(() => IsHighUrgencyRatio);

                if (IsHighUrgencyRatio)
                    UrgencyRatio = OrderDto.HighUrgencyRatio;
            }
        }

        public float UrgencyRatio
        {
            get { return _urgencyRatio; }
            set
            {
                if (_urgencyRatio == value)
                    return;
                _urgencyRatio = value;
                OnPropertyChanged(() => UrgencyRatio);
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
            Customer = order.Customer;
            _responsiblePersonId = order.ResponsiblePersonId;
            ResponsiblePerson = order.ResponsiblePerson;
            Gender = order.Gender;
            Age = order.Age;
            PatientFio = order.Patient;
            Created = order.Created;
            FittingDate = order.FittingDate;
            DateOfCompletion = order.DateOfCompletion;
            DateComment = order.DateComment;
            Stl = order.Stl;
            Cashless = order.Cashless;
            UrgencyRatio = order.UrgencyRatio;

            _isNormalUrgencyRatio = UrgencyRatio == OrderDto.NormalUrgencyRatio;
            _isHighUrgencyRatio = UrgencyRatio == OrderDto.HighUrgencyRatio;
        }

        public override void AssemblyOrder(OrderDto order)
        {
            order.CustomerId = _customerId;
            order.BranchType = BranchType.Laboratory;
            order.Customer = Customer;
            order.ResponsiblePersonId = _responsiblePersonId;
            order.ResponsiblePerson = ResponsiblePerson;
            order.Gender = Gender;
            order.Age = Age;
            order.Patient = PatientFio;
            order.Created = Created; ;
            order.FittingDate = FittingDate;
            order.DateOfCompletion = DateOfCompletion;
            order.DateComment = DateComment;
            order.Stl = Stl;
            order.Cashless = Cashless;
            order.UrgencyRatio = UrgencyRatio;
        }

        private void CustomerRepositoryOnChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(() => IsNewCustomer);
            DelegateCommand.NotifyCanExecuteChangedForAll();
        }

        private void ResponsiblePersonRepositoryOnChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(() => IsNewResponsiblePerson);
            DelegateCommand.NotifyCanExecuteChangedForAll();
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
            var responsiblePerson = _catalogUIOperations.AddResponsiblePerson(new ResponsiblePersonDto { FullName = ResponsiblePerson });
            if (responsiblePerson == null)
                return;

            _responsiblePersonId = responsiblePerson.Id;
        }

        private bool CanShowResponsiblePersonCardCommandHandler()
        {
            if (ResponsiblePerson.IsNullOrWhiteSpace())
                return false;

            return _responsiblePersonRepository.Items.Any(x => x.FullName == ResponsiblePerson);
        }

        private void ShowResponsiblePersonCardCommandHandler()
        {
            _catalogSelectionOperations.ShowResponsiblePersonCard(_responsiblePersonRepository.Items.First(x => x.FullName == ResponsiblePerson));
        }
    }
}