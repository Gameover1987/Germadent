using System;
using System.Linq;
using Germadent.Common.Extensions;
using Germadent.Rma.App.Configuration;
using Germadent.Rma.Model;
using Germadent.Rma.Model.Pricing;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json.Linq;

namespace Germadent.Rma.App.ServiceClient
{
    public class SignalRClient : ISignalRClient
    {
        private readonly IConfiguration _configuration;

        public SignalRClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Dispose()
        {

        }

        public async void Initialize()
        {
            var connection = new HubConnectionBuilder()
                .WithUrl(new Uri(_configuration.DataServiceUrl + "/NotificationHub"))
                .WithAutomaticReconnect()
                .Build();

            await connection.StartAsync();
            connection.On<string>("Send", OnNotification);
        }

        public event EventHandler<RepositoryChangedEventArgs<PriceGroupDto>> PriceGroupRepositoryChanged;

        private void OnNotification(string arg)
        {
            var notification = arg.DeserializeFromJson<RepositoryNotificationDto>();
            if (notification.RepositoryType == RepositoryType.PriceGroup)
            {
                var args = CreateRepositoryChangedEventArgs<PriceGroupDto>(notification);

                PriceGroupRepositoryChanged?.Invoke(this, args);
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