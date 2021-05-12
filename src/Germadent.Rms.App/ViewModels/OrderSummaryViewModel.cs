using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using Germadent.Client.Common.Reporting;
using Germadent.Client.Common.Reporting.PropertyGrid;
using Germadent.Common.Extensions;
using Germadent.Model;

namespace Germadent.Rms.App.ViewModels
{
    public class OrderSummaryViewModel : IOrderSummaryViewModel
    {
        private readonly IPrintableOrderConverter _printableOrderConverter;
        private readonly IPropertyItemsCollector _propertyItemsCollector;
        private OrderDto _order;

        private readonly ICollectionView _propertiesView;

        public OrderSummaryViewModel(IPrintableOrderConverter printableOrderConverter, IPropertyItemsCollector propertyItemsCollector)
        {
            _printableOrderConverter = printableOrderConverter;
            _propertyItemsCollector = propertyItemsCollector;

            _propertiesView = CollectionViewSource.GetDefaultView(PropertyItems);
            _propertiesView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(PropertyItem.GroupName)));
        }

        public string Title => $"Просмотр данных заказ наряда №'{_order.DocNumber}' для {_order.BranchType.GetDescription()}";

        public ObservableCollection<PropertyItem> PropertyItems { get; } = new ObservableCollection<PropertyItem>();

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
        }
    }
}