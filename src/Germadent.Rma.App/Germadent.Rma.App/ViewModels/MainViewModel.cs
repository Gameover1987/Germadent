﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using Germadent.Common.Extensions;
using Germadent.Common.Logging;
using Germadent.Rma.App.Printing;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.Views;
using Germadent.Rma.Model;
using Germadent.UI.Commands;
using Germadent.UI.Infrastructure;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private readonly IRmaOperations _rmaOperations;
        private readonly IWindowManager _windowManager;
        private readonly IShowDialogAgent _dialogAgent;
        private readonly IPrintModule _printModule;
        private readonly ILogger _logger;
        private OrderLiteViewModel _selectedOrder;
        private bool _isBusy;

        public MainViewModel(IRmaOperations rmaOperations, IWindowManager windowManager, IShowDialogAgent dialogAgent, IPrintModule printModule, ILogger logger)
        {
            _rmaOperations = rmaOperations;
            _windowManager = windowManager;
            _dialogAgent = dialogAgent;
            _printModule = printModule;
            _logger = logger;

            SelectedOrder = Orders.FirstOrDefault();

            CreateLabOrderCommand = new DelegateCommand(x => CreateLabOrderCommandHandler());
            CreateMillingCenterOrderCommand = new DelegateCommand(x => CreateMillingCenterOrderCommandHandler());
            FilterOrdersCommand = new DelegateCommand(x => FilterOrdersCommandHandler());
            CloseOrderCommand = new DelegateCommand(x => CloseOrderCommandHandler(), x => CanCloseOrderCommandHandler());
            PrintOrderCommand = new DelegateCommand(x => PrintOrderCommandHandler(), x => CanPrintOrderCommandHandler());
            OpenOrderCommand = new DelegateCommand(x => OpenOrderCommandHandler(), x => CanOpenOrderCommandHandler());
        }

        public ObservableCollection<OrderLiteViewModel> Orders { get; } = new ObservableCollection<OrderLiteViewModel>();

        public OrderLiteViewModel SelectedOrder
        {
            get { return _selectedOrder; }
            set
            {
                if (_selectedOrder == value)
                    return;

                _selectedOrder = value;
                OnPropertyChanged(() => SelectedOrder);
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(() => _isBusy, value); }
        }

        public IDelegateCommand CreateLabOrderCommand { get; }

        public IDelegateCommand CreateMillingCenterOrderCommand { get; }

        public IDelegateCommand FilterOrdersCommand { get; }

        public IDelegateCommand CloseOrderCommand { get; }

        public IDelegateCommand PrintOrderCommand { get; }

        public IDelegateCommand OpenOrderCommand { get; }

        public void Initialize()
        {           
            FillOrders();
        }

        private void CreateLabOrderCommandHandler()
        {
            var labOrder = _windowManager.CreateLabOrder(new OrderDto { BranchType = BranchType.Laboratory }, WizardMode.Create);

            if (labOrder == null)
                return;

            Orders.Add(new OrderLiteViewModel(labOrder.ToOrderLite()));
        }

        private void CreateMillingCenterOrderCommandHandler()
        {
            var millingCenterOrder = _windowManager.CreateMillingCenterOrder(new OrderDto { BranchType = BranchType.MillingCenter}, WizardMode.Create);

            if (millingCenterOrder == null)
                return;
            
            Orders.Add(new OrderLiteViewModel(millingCenterOrder.ToOrderLite()));
        }

        private void FilterOrdersCommandHandler()
        {
            var filter = _windowManager.CreateOrdersFilter();
            if (filter == null)
                return;

            FillOrders(filter);
        }

        private async void FillOrders(OrdersFilter filter = null)
        {
            IsBusy = true;
            try
            {
                Orders.Clear();
                OrderLiteDto[] orders = null;
                await ThreadTaskExtensions.Run(() =>
                {
                    orders = _rmaOperations.GetOrders(filter);
                });

                foreach (var order in orders)
                {
                    Orders.Add(new OrderLiteViewModel(order));
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

        private bool CanCloseOrderCommandHandler()
        {
            return SelectedOrder != null;
        }

        private void CloseOrderCommandHandler()
        {
            if (_dialogAgent.ShowMessageDialog("Вы действительно хотите закрыть заказ наряд?", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            SelectedOrder.Model.Closed = DateTime.Now;
        }

        private bool CanPrintOrderCommandHandler()
        {
            return SelectedOrder != null;
        }

        private void PrintOrderCommandHandler()
        {
            _printModule.Print(_rmaOperations.GetOrderDetails(SelectedOrder.Model.WorkOrderId));
        }

        private bool CanOpenOrderCommandHandler()
        {
            return SelectedOrder != null;
        }

        private void OpenOrderCommandHandler()
        {
            OrderDto changedOrderDto = null;
            var orderLiteViewModel = SelectedOrder;
            var orderDto = _rmaOperations.GetOrderDetails(orderLiteViewModel.Model.WorkOrderId);
            if (orderDto.BranchType == BranchType.Laboratory)
            {
                changedOrderDto = _windowManager.CreateLabOrder(orderDto, WizardMode.Edit);
            }
            else if (orderDto.BranchType == BranchType.MillingCenter)
            {
                changedOrderDto = _windowManager.CreateMillingCenterOrder(orderDto, WizardMode.Edit);
            }

            if (changedOrderDto == null)
                return;
            
            orderLiteViewModel.Update(changedOrderDto.ToOrderLite());
        }
    }
}