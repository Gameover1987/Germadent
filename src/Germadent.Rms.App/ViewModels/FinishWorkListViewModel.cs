﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Germadent.Client.Common.Reporting;
using Germadent.Client.Common.Reporting.PropertyGrid;
using Germadent.Client.Common.ViewModels;
using Germadent.Common.Extensions;
using Germadent.Model;
using Germadent.Model.Production;
using Germadent.Rms.App.ServiceClient;
using Germadent.UI.Commands;

namespace Germadent.Rms.App.ViewModels
{
    public class FinishWorkListViewModel : IFinishWorkListViewModel
    {
        private readonly IPrintableOrderConverter _printableOrderConverter;
        private readonly IPropertyItemsCollector _propertyItemsCollector;
        private readonly IRmsServiceClient _rmsServiceClient;
        private OrderDto _order;

        private readonly ICollectionView _propertiesView;

        public FinishWorkListViewModel(IPrintableOrderConverter printableOrderConverter,
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

        public ObservableCollection<WorkViewModel> Works { get; } = new ObservableCollection<WorkViewModel>();

        public ObservableCollection<WorkViewModel> AllWorks { get; } = new ObservableCollection<WorkViewModel>();

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

            var works = _rmsServiceClient.GetWorksInProgressByWorkOrder(_order.WorkOrderId);
            Works.Clear();
            foreach (var work in works)
            {
                Works.Add(new WorkViewModel(work));
            }

            var allWorks = _rmsServiceClient.GetWorksByWorkOrder(_order.WorkOrderId);
            AllWorks.Clear();
            foreach (var work in allWorks)
            {
                AllWorks.Add(new WorkViewModel(work));
            }
        }

        public WorkDto[] GetWorks()
        {
            var works = Works
                .Where(x => x.IsChecked)
                .Select(x => x.ToDto())
                .ToArray();

            works.ForEach(x => x.UserIdCompleted = _rmsServiceClient.AuthorizationInfo.UserId);
            works.ForEach(x => x.WorkCompleted = DateTime.Now);
            works.ForEach(x => x.WorkOrderId = _order.WorkOrderId);

            return works;
        }

        private bool CanOkCommandHandler()
        {
            return Works.Any(x => x.IsChecked);
        }
    }
}
