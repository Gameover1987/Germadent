using System;
using Germadent.Client.Common.ServiceClient;
using Germadent.Model;
using Germadent.Model.Pricing;
using Germadent.Model.Production;

namespace Germadent.Rma.App.ServiceClient
{
    public class OrderLockedEventArgs
    {
        public OrderLockedEventArgs(OrderLockInfoDto lockInfo)
        {
            Info = lockInfo;
        }

        public OrderLockInfoDto Info { get; }
    }

    public interface ISignalRClient : IDisposable
    {
        void Initialize(AuthorizationInfoDto info);

        event EventHandler<RepositoryChangedEventArgs<PriceGroupDto>> PriceGroupRepositoryChanged;

        event EventHandler<RepositoryChangedEventArgs<CustomerDto>> CustomerRepositoryChanged;

        event EventHandler<RepositoryChangedEventArgs<ResponsiblePersonDto>> ResponsiblePersonRepositoryChanged;

        event EventHandler<RepositoryChangedEventArgs<PricePositionDto>> PricePositionRepositoryChanged;

        event EventHandler<RepositoryChangedEventArgs<ProductDto>> ProductRepositoryChanged;

        event EventHandler<RepositoryChangedEventArgs<TechnologyOperationDto>> TechnologyOperationRepositoryChanged;

        event EventHandler<OrderLockedEventArgs> WorkOrderLockedOrUnlocked;
    }
}