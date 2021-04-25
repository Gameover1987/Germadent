using System;
using System.Linq;
using Germadent.Common.Extensions;
using Germadent.Rma.App.Configuration;
using Germadent.Rma.Model;
using Germadent.Rma.Model.Pricing;
using Germadent.Rma.Model.Production;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json.Linq;

namespace Germadent.Rma.App.ServiceClient
{
    public class SignalRClient : ISignalRClient
    {
        private readonly IConfiguration _configuration;

        private HubConnection _connection;

        public SignalRClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async void Dispose()
        {
            if (_connection == null)
                return;
            await _connection.DisposeAsync();
        }

        public async void Initialize()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(new Uri(_configuration.DataServiceUrl + "/NotificationHub"))
                .WithAutomaticReconnect()
                .Build();

            await _connection.StartAsync();
            _connection.On<string>("Send", OnNotification);
        }

        public event EventHandler<RepositoryChangedEventArgs<PriceGroupDto>> PriceGroupRepositoryChanged;
        public event EventHandler<RepositoryChangedEventArgs<CustomerDto>> CustomerRepositoryChanged;
        public event EventHandler<RepositoryChangedEventArgs<ResponsiblePersonDto>> ResponsiblePersonRepositoryChanged;
        public event EventHandler<RepositoryChangedEventArgs<PricePositionDto>> PricePositionRepositoryChanged;
        public event EventHandler<RepositoryChangedEventArgs<ProductDto>> ProductRepositoryChanged;
        public event EventHandler<RepositoryChangedEventArgs<TechnologyOperationDto>> TechnologyOperationRepositoryChanged;

        private void OnNotification(string arg)
        {
            var notification = arg.DeserializeFromJson<RepositoryNotificationDto>();

            if (notification.RepositoryType == RepositoryType.Customer)
            {
                var args = CreateRepositoryChangedEventArgs<CustomerDto>(notification);
                CustomerRepositoryChanged?.Invoke(this, args);
            }

            if (notification.RepositoryType == RepositoryType.ResponsiblePerson)
            {
                var args = CreateRepositoryChangedEventArgs<ResponsiblePersonDto>(notification);
                ResponsiblePersonRepositoryChanged?.Invoke(this, args);
            }

            if (notification.RepositoryType == RepositoryType.PriceGroup)
            {
                var args = CreateRepositoryChangedEventArgs<PriceGroupDto>(notification);
                PriceGroupRepositoryChanged?.Invoke(this, args);
            }

            if (notification.RepositoryType == RepositoryType.PricePosition)
            {
                var args = CreateRepositoryChangedEventArgs<PricePositionDto>(notification);
                PricePositionRepositoryChanged?.Invoke(this, args);
            }

            if (notification.RepositoryType == RepositoryType.Product)
            {
                var args = CreateRepositoryChangedEventArgs<ProductDto>(notification);
                ProductRepositoryChanged?.Invoke(this, args);
            }

            if (notification.RepositoryType == RepositoryType.TechnologyOperation)
            {
                var args = CreateRepositoryChangedEventArgs<TechnologyOperationDto>(notification);
                TechnologyOperationRepositoryChanged?.Invoke(this, args);
            }
        }

        private static RepositoryChangedEventArgs<T> CreateRepositoryChangedEventArgs<T>(RepositoryNotificationDto notification)
        {
            T[] addedItems = null;
            T[] changedItems = null;
            int[] deletedIds = null;

            if (notification.AddedItems != null)
                addedItems = notification.AddedItems.Select(x => ((JObject)x).ToObject<T>()).ToArray();

            if (notification.ChangedItems != null)
                changedItems = notification.ChangedItems.Select(x => ((JObject)x).ToObject<T>()).ToArray();

            if (notification.DeletedItems != null)
                deletedIds = notification.DeletedItems.Cast<int>().ToArray();

            return new RepositoryChangedEventArgs<T>(addedItems, changedItems, deletedIds);
        }
    }
}