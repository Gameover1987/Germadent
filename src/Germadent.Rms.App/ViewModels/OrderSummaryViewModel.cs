using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Germadent.Client.Common.Reporting;
using Germadent.Client.Common.Reporting.PropertyGrid;
using Germadent.Common.Extensions;
using Germadent.Model;
using Germadent.Model.Production;
using Germadent.Rms.App.ServiceClient;
using Germadent.UI.Commands;

namespace Germadent.Rms.App.ViewModels
{
    public class OrderSummaryViewModel : IOrderSummaryViewModel
    {
        private readonly IPrintableOrderConverter _printableOrderConverter;
        private readonly IPropertyItemsCollector _propertyItemsCollector;
        private readonly IRmsServiceClient _rmsServiceClient;
        private OrderDto _order;

        private readonly ICollectionView _propertiesView;

        public OrderSummaryViewModel(IPrintableOrderConverter printableOrderConverter,
            IPropertyItemsCollector propertyItemsCollector,
            IRmsServiceClient rmsServiceClient)
        {
            _printableOrderConverter = printableOrderConverter;
            _propertyItemsCollector = propertyItemsCollector;
            _rmsServiceClient = rmsServiceClient;

            _propertiesView = CollectionViewSource.GetDefaultView(PropertyItems);
            _propertiesView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(PropertyItem.GroupName)));

            OkCommand = new DelegateCommand(CanOkCommandHandler);
        }

        public string Title => $"Просмотр данных заказ наряда №'{_order.DocNumber}' для {_order.BranchType.GetDescription()}";

        public ObservableCollection<PropertyItem> PropertyItems { get; } = new ObservableCollection<PropertyItem>();

        public ObservableCollection<TechnologyOperationByUserViewModel> Operations { get; } = new ObservableCollection<TechnologyOperationByUserViewModel>();

        public IDelegateCommand OkCommand { get; }

        public void Initialize(OrderDto orderDto)
        {
            _order = orderDto;

            var printableOrder = _printableOrderConverter.ConvertFrom(_order);
            var propertyItems = _propertyItemsCollector.GetProperties(printableOrder);
            PropertyItems.Clear();
            foreach (var propertyItem in propertyItems)
            {
                PropertyItems.Add(propertyItem);
            }

            var operations = _rmsServiceClient.GetRelevantWorkListByWorkOrder(_order.WorkOrderId);
            Operations.Clear();
            foreach (var technologyOperationByUserDto in operations)
            {
                Operations.Add(new TechnologyOperationByUserViewModel(technologyOperationByUserDto));
            }
        }

        public WorkDto[] GetWorks()
        {
            var works = new List<WorkDto>();

            var selectedOperations = Operations.Where(x => x.IsChecked).ToArray();
            foreach (var operation in selectedOperations)
            {
                var technologyOperationByUser = operation.ToDto();
                var work = new WorkDto()
                {
                    WorkOrderId = _order.WorkOrderId,
                    EmployeeId = _rmsServiceClient.AuthorizationInfo.UserId,
                    ProductId = technologyOperationByUser.ProductId,
                    Quantity = technologyOperationByUser.ProductCount,
                    Rate = technologyOperationByUser.Rate,
                    TechnologyOperationId = technologyOperationByUser.Operation.TechnologyOperationId,
                    UrgencyRatio = technologyOperationByUser.UrgencyRatio,
                    TechnologyOperationName = technologyOperationByUser.Operation.Name,
                    TechnologyOperationUserCode = technologyOperationByUser.Operation.UserCode,
                    OperationCost = technologyOperationByUser.TotalCost,
                    WorkStarted = DateTime.Now
                };
                works.Add(work);
            }

            return works.ToArray();
        }

        private bool CanOkCommandHandler()
        {
            return Operations.Any(x => x.IsChecked);
        }
    }
}