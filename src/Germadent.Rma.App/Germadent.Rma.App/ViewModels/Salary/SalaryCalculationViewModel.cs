using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Germadent.Client.Common.Infrastructure;
using Germadent.Common.Extensions;
using Germadent.Model;
using Germadent.Model.Production;
using Germadent.Rma.App.ServiceClient;
using Germadent.UI.Commands;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.Salary
{
    public class SalaryCalculationViewModel : ViewModelBase, ISalaryCalculationViewModel
    {
        private readonly IRmaServiceClient _rmaServiceClient;
        private readonly ICommandExceptionHandler _commandExceptionHandler;
        private EmployeeViewModel _selectedEmployee;
        private DateTime _dateFrom;
        private DateTime _dateTo;
        private string _datesValidationError;

        private readonly ICollectionView _salaryView;
        private string _busyReason;
        private bool _isBusy;

        public SalaryCalculationViewModel(IRmaServiceClient rmaServiceClient, ICommandExceptionHandler commandExceptionHandler)
        {
            _rmaServiceClient = rmaServiceClient;
            _commandExceptionHandler = commandExceptionHandler;

            Works.CollectionChanged+= WorksOnCollectionChanged;
            _salaryView = CollectionViewSource.GetDefaultView(Works);

            CalculateSalaryCommand = new DelegateCommand(CalculateSalaryCommandHandler, CanCalculateSalaryCommandHandler);
        }

        public ObservableCollection<EmployeeViewModel> Employees { get; } = new ObservableCollection<EmployeeViewModel>();

        public EmployeeViewModel SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                if (_selectedEmployee == value)
                    return;
                _selectedEmployee = value;
                OnPropertyChanged(() => SelectedEmployee);
            }
        }

        public ObservableCollection<WorkViewModel> Works { get; } = new ObservableCollection<WorkViewModel>();

        public decimal Salary
        {
            get { return Works.Sum(x => x.OperationCost); }
        }

        public string BusyReason
        {
            get { return _busyReason; }
            set
            {
                if (_busyReason == value)
                    return;
                _busyReason = value;
                OnPropertyChanged(() => BusyReason);

                IsBusy = BusyReason != null;
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy == value)
                    return;
                _isBusy = value;
                OnPropertyChanged(() => IsBusy);
            }
        }

        public DateTime DateFrom
        {
            get => _dateFrom;
            set
            {
                if (_dateFrom == value)
                    return;
                _dateFrom = value;
                OnPropertyChanged(() => DateFrom);

                ValidateDates();
            }
        }

        public DateTime DateTo
        {
            get => _dateTo;
            set
            {
                if (_dateTo == value)
                    return;
                _dateTo = value;
                OnPropertyChanged(() => DateTo);

                ValidateDates();
            }
        }

        public string DatesValidationError
        {
            get => _datesValidationError;
            set
            {
                if (_datesValidationError == value)
                    return;
                _datesValidationError = value;
                OnPropertyChanged(() => DatesValidationError);
                OnPropertyChanged(() => IsValid);
            }
        }

        public bool IsValid => DatesValidationError == null;

        public IDelegateCommand CalculateSalaryCommand { get; }

        public async void Initialize()
        {
            _dateFrom = DateTime.Now.AddMonths(-1).Date;
            _dateTo = DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59);

            Employees.Clear();
            Works.Clear();

            try
            {
                BusyReason = "Инициализация";
                UserDto[] allUsers = null;
                await ThreadTaskExtensions.Run(() =>
                {
                    allUsers = _rmaServiceClient
                        .GetAllUsers()
                        .Where(x => x.IsEmployee)
                        .OrderBy(x => x.FullName)
                        .ToArray();
                });

                Employees.Add(AllEmployeeViewModel.Instance);
                foreach (var userDto in allUsers)
                {
                    Employees.Add(new EmployeeViewModel(userDto));
                }

                SelectedEmployee = AllEmployeeViewModel.Instance;
            }
            catch (Exception exception)
            {
                _commandExceptionHandler.HandleCommandException(exception);
            }
            finally
            {
                BusyReason = null;
            }
        }

        private void ValidateDates()
        {
            DatesValidationError = null;
            if (DateFrom > DateTo)
            {
                DatesValidationError = "Дата начала периода расчета должна быть меньше даты окончания периода расчета";
            }

            if (DateTo < DateFrom)
            {
                DatesValidationError = "Дата окончания периода расчета должна быть больше даты начала периода расчета";
            }
        }

        private bool CanCalculateSalaryCommandHandler()
        {
            if (IsBusy)
                return false;

            return SelectedEmployee != null && DatesValidationError == null;
        }

        private async void CalculateSalaryCommandHandler()
        {
            try
            {
                BusyReason = $"Расчет заработной платы";
                Works.Clear();

                _salaryView.GroupDescriptions.Clear();
                if (SelectedEmployee == AllEmployeeViewModel.Instance)
                {
                    _salaryView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(WorkViewModel.UserFullName)));
                }

                WorkDto[] salaryReport = null;
                await ThreadTaskExtensions.Run(() =>
                {
                    salaryReport = _rmaServiceClient.GetSalaryReport(SelectedEmployee.UserId, DateFrom, DateTo);
                });

                foreach (var workDto in salaryReport)
                {
                    Works.Add(new WorkViewModel(workDto));
                }
            }
            catch (Exception exception)
            {
                _commandExceptionHandler.HandleCommandException(exception);
            }
            finally
            {
                BusyReason = null;
            }
        }

        private void WorksOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(() => Salary);
        }
    }
}