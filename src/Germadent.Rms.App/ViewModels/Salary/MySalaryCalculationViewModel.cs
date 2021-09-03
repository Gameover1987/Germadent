using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Germadent.Client.Common.Infrastructure;
using Germadent.Client.Common.Reporting;
using Germadent.Client.Common.ViewModels;
using Germadent.Client.Common.ViewModels.Salary;
using Germadent.Common.Extensions;
using Germadent.Model;
using Germadent.Model.Production;
using Germadent.Rms.App.ServiceClient;
using Germadent.UI.Commands;
using Germadent.UI.ViewModels;

namespace Germadent.Rms.App.ViewModels.Salary
{
    public class MySalaryCalculationViewModel : ViewModelBase, ISalaryCalculationViewModel
    {
        private readonly IRmsServiceClient _rmsServiceClient;
        private readonly ICommandExceptionHandler _commandExceptionHandler;
        private readonly IPrintModule _printModule;

        private DateTime _dateFrom;
        private DateTime _dateTo;
        private string _datesValidationError;

        private readonly ICollectionView _salaryView;
        private string _busyReason;
        private bool _isBusy;
        private SalaryReport _salaryReport;

        public MySalaryCalculationViewModel(IRmsServiceClient rmsServiceClient, ICommandExceptionHandler commandExceptionHandler, IPrintModule printModule)
        {
            _rmsServiceClient = rmsServiceClient;
            _commandExceptionHandler = commandExceptionHandler;
            _printModule = printModule;

            Works.CollectionChanged += WorksOnCollectionChanged;
            _salaryView = CollectionViewSource.GetDefaultView(Works);

            CalculateSalaryCommand = new DelegateCommand(CalculateSalaryCommandHandler, CanCalculateSalaryCommandHandler);
            PrintSalaryReportCommand = new DelegateCommand(SaveSalaryReportCommandHandler, CanSaveSalaryReportCommandHandler);
        }

        public ObservableCollection<WorkViewModel> Works { get; } = new ObservableCollection<WorkViewModel>();

        public int TotalQuantity
        {
            get { return Works.Sum(x => x.Quantity); }
        }
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

        public IDelegateCommand PrintSalaryReportCommand { get; }

        public void Initialize()
        {
            _dateFrom = DateTime.Now.AddMonths(-1).Date;
            _dateTo = DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59);
            
            Works.Clear();

            Title = $"Расчет заработной платы по сотруднику '{_rmsServiceClient.AuthorizationInfo.GetShortFullName()}'";
        }

        public string Title { get;  private set; }

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

            return DatesValidationError == null;
        }

        private async void CalculateSalaryCommandHandler()
        {
            try
            {
                BusyReason = "Расчет заработной платы...";
                Works.Clear();
                _salaryReport = null;

                _salaryView.GroupDescriptions.Clear();

                WorkDto[] works = null;
                await ThreadTaskExtensions.Run(() =>
                {
                    works = _rmsServiceClient.GetSalaryReport(_rmsServiceClient.AuthorizationInfo.UserId, DateFrom, DateTo);
                });

                foreach (var workDto in works)
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
                DelegateCommand.NotifyCanExecuteChangedForAll();
            }
        }

        private bool CanSaveSalaryReportCommandHandler()
        {
            return _salaryReport != null && _salaryReport.Works.Any();
        }

        private void SaveSalaryReportCommandHandler()
        {
            _printModule.PrintSalaryReport(_salaryReport);
        }
        private void WorksOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(() => TotalQuantity);
            OnPropertyChanged(() => Salary);
        }
    }
}
