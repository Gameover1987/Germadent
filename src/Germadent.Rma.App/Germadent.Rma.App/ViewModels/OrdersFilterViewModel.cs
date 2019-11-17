using System;
using System.Collections.ObjectModel;
using System.Linq;
using Germadent.ServiceClient.Model;
using Germadent.ServiceClient.Operation;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels
{
    public interface IOrdersFilterViewModel
    {
        OrdersFilter GetFilter();
    }

    public class OrdersFilterViewModel : ViewModelBase, IOrdersFilterViewModel
    {
        private readonly IRmaOperations _rmaOperations;

        private bool _millingCenter;
        private bool _laboratory;
        private DateTime _periodBegin;
        private DateTime _periodEnd;
        private string _customer;
        private string _employee;
        private string _patient;

        public OrdersFilterViewModel(IRmaOperations rmaOperations)
        {
            _rmaOperations = rmaOperations;
        }

        public bool MillingCenter
        {
            get { return _millingCenter;}
            set { SetProperty(() => _millingCenter, value); }
        }

        public bool Laboratory
        {
            get { return _laboratory;}
            set { SetProperty(() => _laboratory, value); }
        }

        public DateTime PeriodBegin
        {
            get { return _periodBegin;}
            set { SetProperty(() => _periodBegin, value); }
        }

        public DateTime PeriodEnd
        {
            get { return _periodEnd;}
            set { SetProperty(() => _periodEnd, value); }
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
            get { return _patient;}
            set { SetProperty(() => _patient, value); }
        }

        public ObservableCollection<MaterialViewModel> Materials { get; } = new ObservableCollection<MaterialViewModel>();

        public OrdersFilter GetFilter()
        {
            var filter= new OrdersFilter
            {
                MillingCenter = MillingCenter,
                Laboratory = Laboratory,
                Customer = Customer,
                Employee = Employee,
                Materials = Materials.Where(x => x.IsChecked).Select(x => x.Item).ToArray()
            };
            return filter;
        }
    }
}
