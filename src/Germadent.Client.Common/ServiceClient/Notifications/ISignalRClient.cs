﻿using System;
using Germadent.Client.Common.ServiceClient.Repository;
using Germadent.Model;
using Germadent.Model.Pricing;
using Germadent.Model.Production;

namespace Germadent.Client.Common.ServiceClient.Notifications
{
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

        event EventHandler<OrderStatusChangedEventArgs> WorkOrderStatusChanged;
    }
}