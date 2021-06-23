using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Data;
using Germadent.Client.Common.Infrastructure;
using Germadent.Client.Common.Reporting;
using Germadent.Client.Common.Reporting.PropertyGrid;
using Germadent.Client.Common.ViewModels;
using Germadent.Common.Extensions;
using Germadent.Model;
using Germadent.Model.Production;
using Germadent.Rms.App.ServiceClient;
using Germadent.UI.Commands;
using Germadent.UI.ViewModels;

namespace Germadent.Rms.App.ViewModels
{
    public class OrderDetailsViewModel : ViewModelBase, IOrderDetailsViewModel
    {
        private readonly IPrintableOrderConverter _printableOrderConverter;
        private readonly IPropertyItemsCollector _propertyItemsCollector;
        private readonly IRmsServiceClient _rmsServiceClient;
        private readonly ICommandExceptionHandler _commandExceptionHandler;
        private OrderDto _order;
        private bool _isBusy;

        public OrderDetailsViewModel(IPrintableOrderConverter printableOrderConverter,
            IPropertyItemsCollector propertyItemsCollector,
            IRmsServiceClient rmsServiceClient,
            ICommandExceptionHandler commandExceptionHandler)
        {
            _printableOrderConverter = printableOrderConverter;
            _propertyItemsCollector = propertyItemsCollector;
            _rmsServiceClient = rmsServiceClient;
            _commandExceptionHandler = commandExceptionHandler;

            var propertiesView = CollectionViewSource.GetDefaultView(PropertyItems);
            propertiesView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(PropertyItem.GroupName)));
        }

        public string Title
        {
            get
            {
                if (_order == null)
                    return null;
                
                return $"Просмотр данных заказ наряда №'{_order.DocNumber}' для {_order.BranchType.GetDescription()}";
            }
        } 

        public ObservableCollection<PropertyItem> PropertyItems { get; } = new ObservableCollection<PropertyItem>();

        public ObservableCollection<WorkViewModel> Works { get; } = new ObservableCollection<WorkViewModel>();
        
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

        public async void Initialize(int workOrderId)
        {
            try
            {
                IsBusy = true;

                await ThreadTaskExtensions.Run(() =>
                {
                    using (var orderScope = _rmsServiceClient.GetOrderById(workOrderId))
                    {
                        _order = orderScope.Order;
                    }
                });
                
                OnPropertyChanged(() => Title);
            }
            catch (Exception exception)
            {
                _commandExceptionHandler.HandleCommandException(exception);
            }
            finally
            {
                IsBusy = false;
            }

            var printableOrder = _printableOrderConverter.ConvertFrom(_order);
            var propertyItems = _propertyItemsCollector.GetProperties(printableOrder);
            PropertyItems.Clear();
            foreach (var propertyItem in propertyItems)
            {
                PropertyItems.Add(propertyItem);
            }

            var works = _rmsServiceClient
                .GetWorksByWorkOrder(_order.WorkOrderId)
                .OrderBy(x => x.WorkStarted)
                .ToArray();
            Works.Clear();
            foreach (var technologyOperationByUserDto in works)
            {
                Works.Add(new WorkViewModel(technologyOperationByUserDto));
            }
        }
    }
}