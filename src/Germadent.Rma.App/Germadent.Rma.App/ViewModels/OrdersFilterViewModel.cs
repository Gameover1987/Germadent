using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using Germadent.Common.Extensions;
using Germadent.Common.Logging;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ViewModels.ToothCard;
using Germadent.Rma.Model;
using Germadent.UI.Commands;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels
{
    public interface IOrdersFilterViewModel
    {
        OrdersFilter GetFilter();

        void Initialize();
    }

    public class OrdersFilterViewModel : ViewModelBase, IOrdersFilterViewModel
    {
        private readonly IRmaOperations _rmaOperations;
        private readonly ILogger _logger;

        private bool _millingCenter;
        private bool _laboratory;
        private DateTime _periodBegin;
        private DateTime _periodEnd;
        private string _customer;
        private string _employee;
        private string _patient;

        public string PeriodValidationError = "Дата начала периода должна быть меньше даты окончания";
        public string DepartmentValidationError = "Необходимо выбрать хотя бы одно подразделение";
        private bool _isBusy;

        public OrdersFilterViewModel(IRmaOperations rmaOperations, ILogger logger)
        {
            _rmaOperations = rmaOperations;
            _logger = logger;

            _millingCenter = true;
            _laboratory = true;

            var now = DateTime.Now.Date;
            _periodBegin = now.AddDays(-30);
            _periodEnd = now.AddHours(23).AddMinutes(59).AddSeconds(59);

            OKCommand = new DelegateCommand(x => { }, x => CanOkCommandHandler());
        }

        public bool MillingCenter
        {
            get { return _millingCenter; }
            set
            {
                if (SetProperty(() => _millingCenter, value))
                {
                    ValidateDepartments();
                }
            }
        }

        public bool Laboratory
        {
            get { return _laboratory; }
            set
            {
                if (SetProperty(() => _laboratory, value))
                {
                    ValidateDepartments();
                }
            }
        }

        public DateTime PeriodBegin
        {
            get { return _periodBegin; }
            set
            {
                if (SetProperty(() => _periodBegin, value))
                {
                    ValidateDates();
                }
            }
        }

        public DateTime PeriodEnd
        {
            get { return _periodEnd; }
            set
            {
                if (SetProperty(() => _periodEnd, value))
                {
                    ValidateDates();
                }
            }
        }

        public string Customer
        {
            get { return _customer; }
            set { SetProperty(() => _customer, value); }
        }

        public string Employee
        {
            get { return _employee; }
            set { SetProperty(() => _employee, value); }
        }

        public string Patient
        {
            get { return _patient; }
            set { SetProperty(() => _patient, value); }
        }

        public ObservableCollection<MaterialViewModel> Materials { get; } = new ObservableCollection<MaterialViewModel>();

        public ObservableCollection<string> ValidationErrors { get; } = new ObservableCollection<string>();

        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(() => _isBusy, value); }
        }

        public bool IsValid
        {
            get { return ValidationErrors.Count == 0; }
        }

        public string LastError
        {
            get { return ValidationErrors.LastOrDefault(); }
        }

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
                Doctor = Employee,
                Patient = Patient,
                Materials = Materials.Where(x => x.IsChecked).Select(x => x.Item).ToArray()
            };
            return filter;
        }

        public async void Initialize()
        {
            try
            {
                IsBusy = true;
                Materials.Clear();

                MaterialDto[] materials = null;
                await ThreadTaskExtensions.Run(() =>
                {
                    materials = _rmaOperations.GetMaterials();
                });

                foreach (var material in materials)
                {
                    Materials.Add(new MaterialViewModel(material));
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
