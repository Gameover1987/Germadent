using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Germadent.Client.Common.ServiceClient;
using Germadent.Client.Common.ServiceClient.Repository;
using Germadent.Client.Common.ViewModels;
using Germadent.Common.Logging;
using Germadent.Model;
using Germadent.Rms.App.ServiceClient;
using Germadent.UI.Commands;
using Germadent.UI.ViewModels;

namespace Germadent.Rms.App.ViewModels
{
    public class OrdersFilterViewModel : ViewModelBase, IOrdersFilterViewModel
    {
        private readonly IDictionaryRepository _dictionaryRepository;
        private readonly ILogger _logger;
        private readonly IRmsServiceClient _rmsServiceClient;

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
        private bool _showCreated;
        private bool _showInProgress;
        private bool _showQualityControl;
        private bool _showRealization;
        private bool _showClosed;
        private bool _showOnlyMyOrders;

        public OrdersFilterViewModel(IDictionaryRepository dictionaryRepository, ILogger logger, IRmsServiceClient rmsServiceClient)
        {
            _dictionaryRepository = dictionaryRepository;
            _logger = logger;
            _rmsServiceClient = rmsServiceClient;

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

        public bool ShowCreated
        {
            get { return _showCreated; }
            set
            {
                if (_showCreated == value)
                    return;
                _showCreated = value;
                OnPropertyChanged(() => ShowCreated);
            }
        }

        public bool ShowInProgress
        {
            get { return _showInProgress; }
            set
            {
                if (_showInProgress == value)
                    return;
                _showInProgress = value;
                OnPropertyChanged(() => ShowInProgress);
            }
        }

        public bool ShowQualityControl
        {
            get { return _showQualityControl; }
            set
            {
                if (_showQualityControl == value)
                    return;
                _showQualityControl = value;
                OnPropertyChanged(() => ShowQualityControl);
            }
        }

        public bool ShowRealization
        {
            get { return _showRealization; }
            set
            {
                if (_showRealization == value)
                    return;
                _showRealization = value;
                OnPropertyChanged(() => ShowQualityControl);
            }
        }

        public bool ShowClosed
        {
            get { return _showClosed; }
            set
            {
                if (_showClosed == value)
                    return;
                _showClosed = value;
                OnPropertyChanged(() => ShowClosed);
            }
        }

        public int? UserId { get; private set; }

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

        public bool ShowOnlyMyOrders
        {
            get { return _showOnlyMyOrders; }
            set
            {
                if (_showOnlyMyOrders == value)
                    return;
                _showOnlyMyOrders = value;
                OnPropertyChanged(() => ShowOnlyMyOrders);
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
            var statuses = new List<OrderStatus>();
            if (ShowCreated)
                statuses.Add(OrderStatus.Created);
            if (ShowInProgress)
                statuses.Add(OrderStatus.InProgress);
            if (ShowQualityControl)
                statuses.Add(OrderStatus.QualityControl);
            if (ShowRealization)
                statuses.Add(OrderStatus.Realization);
            if (ShowClosed)
                statuses.Add(OrderStatus.Closed);
            var filter = new OrdersFilter
            {
                MillingCenter = MillingCenter,
                Laboratory = Laboratory,
                PeriodBegin = PeriodBegin,
                PeriodEnd = PeriodEnd,
                Customer = Customer,
                Doctor = Doctor,
                Patient = Patient,
                Materials = Materials.Where(x => x.IsChecked).Select(x => x.Item).ToArray(),
                Statuses = statuses.ToArray(),
                ShowOnlyMyOrders = ShowOnlyMyOrders,
                UserId = _rmsServiceClient.AuthorizationInfo.UserId
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

                ShowCreated = filter.Statuses.Any(x => x == OrderStatus.Created);
                ShowInProgress = filter.Statuses.Any(x => x == OrderStatus.InProgress);
                ShowQualityControl = filter.Statuses.Any(x => x == OrderStatus.QualityControl);
                ShowRealization = filter.Statuses.Any(x => x == OrderStatus.Realization);
                ShowClosed = filter.Statuses.Any(x => x == OrderStatus.Closed);
                UserId = filter.UserId;
                ShowOnlyMyOrders = filter.ShowOnlyMyOrders;
                //ShowOnlyMyOrders = filter.UserId == _rmsServiceClient.AuthorizationInfo.UserId;
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
