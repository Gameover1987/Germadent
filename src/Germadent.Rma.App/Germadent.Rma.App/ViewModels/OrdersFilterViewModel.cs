using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Germadent.Common.Logging;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.App.ViewModels.ToothCard;
using Germadent.Rma.Model;
using Germadent.UI.Commands;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels
{
    public class OrdersFilterViewModel : ViewModelBase, IOrdersFilterViewModel
    {
        private readonly IDictionaryRepository _dictionaryRepository;
        private readonly ILogger _logger;

        private bool _millingCenter;
        private bool _laboratory;
        private DateTime _periodBegin;
        private DateTime _periodEnd;
        private string _customer;
        private string _doctor;
        private string _patient;

        public string PeriodValidationError = "Дата начала периода должна быть меньше даты окончания";
        public string DepartmentValidationError = "Необходимо выбрать хотя бы одно подразделение";
        private bool _isBusy;

        public OrdersFilterViewModel(IDictionaryRepository dictionaryRepository, ILogger logger)
        {
            _dictionaryRepository = dictionaryRepository;
            _logger = logger;

            _millingCenter = true;
            _laboratory = true;

            OKCommand = new DelegateCommand(x => { }, x => CanOkCommandHandler());
        }

        public bool MillingCenter
        {
            get => _millingCenter;
            set
            {
                if (_millingCenter == value)
                    return;
                _millingCenter = value;
                OnPropertyChanged(() => MillingCenter);

                ValidateDepartments();
            }
        }

        public bool Laboratory
        {
            get => _laboratory;
            set
            {
                if (_laboratory == value)
                    return;
                _laboratory = value;
                OnPropertyChanged(() => Laboratory);

                ValidateDepartments();
            }
        }

        public DateTime PeriodBegin
        {
            get => _periodBegin;
            set
            {
                if (_periodBegin == value)
                    return;
                _periodBegin = value;
                OnPropertyChanged(() => PeriodBegin);
                ValidateDates();
            }
        }

        public DateTime PeriodEnd
        {
            get => _periodEnd;
            set
            {
                if (_periodEnd == value)
                    return;
                _periodEnd = value;
                OnPropertyChanged(() => PeriodEnd);
                ValidateDates();
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
            }
        }

        public string Doctor
        {
            get => _doctor;
            set
            {
                if (_doctor == value)
                    return;
                _doctor = value;
                OnPropertyChanged(() => Doctor);
            }
        }

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

        public ObservableCollection<CheckableDictionaryItemViewModel> Materials { get; } = new ObservableCollection<CheckableDictionaryItemViewModel>();

        public ObservableCollection<string> ValidationErrors { get; } = new ObservableCollection<string>();

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy == value)
                    return;
                _isBusy = value;
                OnPropertyChanged(() => IsBusy);
            }
        }

        public bool IsValid => ValidationErrors.Count == 0;

        public string LastError => ValidationErrors.LastOrDefault();

        public ICommand OKCommand { get; }


        private bool CanOkCommandHandler()
        {
            return IsValid;
        }

        private void ValidateDates()
        {
            if (PeriodEnd < PeriodBegin)
            {
                ValidationErrors.Add(PeriodValidationError);
            }
            else
            {
                ValidationErrors.Remove(PeriodValidationError);
            }

            OnPropertyChanged(() => IsValid);
            OnPropertyChanged(() => LastError);
        }

        private void ValidateDepartments()
        {
            if (Laboratory == false &&
                MillingCenter == false)
            {
                ValidationErrors.Add(DepartmentValidationError);
            }
            else
            {
                ValidationErrors.Remove(DepartmentValidationError);
            }

            OnPropertyChanged(() => IsValid);
            OnPropertyChanged(() => LastError);
        }

        public OrdersFilter GetFilter()
        {
            var filter = new OrdersFilter
            {
                MillingCenter = MillingCenter,
                Laboratory = Laboratory,
                PeriodBegin = PeriodBegin,
                PeriodEnd = PeriodEnd,
                Customer = Customer,
                Doctor = Doctor,
                Patient = Patient,
                Materials = Materials.Where(x => x.IsChecked).Select(x => x.Item).ToArray()
            };
            return filter;
        }

        public void Initialize(OrdersFilter filter)
        {
            try
            {
                IsBusy = true;

                Laboratory = filter.Laboratory;
                MillingCenter = filter.MillingCenter;
                PeriodBegin = filter.PeriodBegin;
                PeriodEnd = filter.PeriodEnd;
                Customer = filter.Customer;
                Doctor = filter.Doctor;
                Patient = filter.Patient;

                var selectedMaterialIds = filter.Materials.Select(x => x.Id).ToArray();

                Materials.Clear();
                foreach (var material in _dictionaryRepository.GetItems(DictionaryType.Material))
                {
                    var checkableDictionaryItemViewModel = new CheckableDictionaryItemViewModel(material);
                    checkableDictionaryItemViewModel.IsChecked = selectedMaterialIds.Contains(material.Id);
                    Materials.Add(checkableDictionaryItemViewModel);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
