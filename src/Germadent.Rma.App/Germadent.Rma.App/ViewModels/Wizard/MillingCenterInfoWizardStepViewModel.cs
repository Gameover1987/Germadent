using System;
using System.Linq;
using Germadent.Common.Extensions;
using Germadent.Rma.App.Operations;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;
using Germadent.Rma.Model;
using Germadent.UI.Commands;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public class MillingCenterInfoWizardStepViewModel : WizardStepViewModelBase
    {
        private readonly ICatalogSelectionUIOperations _catalogSelectionOperations;
        private readonly ICatalogUIOperations _catalogUIOperations;
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
        private bool _stl;
        private bool _cashless;

        public MillingCenterInfoWizardStepViewModel(ICatalogSelectionUIOperations catalogSelectionOperations,
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

            SelectCustomerCommand = new DelegateCommand(SelectCustomerCommandHandler);
            AddCustomerCommand = new DelegateCommand(AddCustomerCommandHandler, CanAddCustomerCommandHandler);
            ShowCustomerCardCommand = new DelegateCommand(ShowCustomerCardCommandHandler, CanShowCustomerCardCommandHandler);

            SelectResponsiblePersonCommand = new DelegateCommand(SelectResponsiblePersonCommandHandler);
            AddResponsiblePersonCommand = new DelegateCommand(AddResponsiblePersonCommandHandler, CanAddResponsiblePersonCommandHandler);
            ShowResponsiblePersonCardCommand = new DelegateCommand(ShowResponsiblePersonCardCommandHandler, CanShowResponsiblePersonCardCommandHandler);
        }

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

                if (IsNewResponsiblePerson)
                    return false;

                return true;
            }
        }

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
            _stl = order.Stl;
            _cashless = order.Cashless;
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
            order.Stl = Stl;
            order.Cashless = Cashless;
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
            var responsiblePerson = _catalogUIOperations.AddResponsiblePerson(new ResponsiblePersonDto {FullName = ResponsiblePerson});
            if (responsiblePerson==null)
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
