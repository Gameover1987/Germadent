using System;
using Germadent.Client.Common.ServiceClient;
using Germadent.Client.Common.ServiceClient.Notifications;
using Germadent.Client.Common.ServiceClient.Repository;
using Germadent.Model;
using Germadent.Model.Pricing;
using Germadent.Model.Production;
using Germadent.Rma.App.ServiceClient;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockSignalRClient : ISignalRClient
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Initialize(AuthorizationInfoDto info)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<RepositoryChangedEventArgs<PriceGroupDto>> PriceGroupRepositoryChanged;
        public event EventHandler<RepositoryChangedEventArgs<CustomerDto>> CustomerRepositoryChanged;
        public event EventHandler<RepositoryChangedEventArgs<ResponsiblePersonDto>> ResponsiblePersonRepositoryChanged;
        public event EventHandler<RepositoryChangedEventArgs<PricePositionDto>> PricePositionRepositoryChanged;
        public event EventHandler<RepositoryChangedEventArgs<ProductDto>> ProductRepositoryChanged;
        public event EventHandler<RepositoryChangedEventArgs<TechnologyOperationDto>> TechnologyOperationRepositoryChanged;
        public event EventHandler<OrderLockedEventArgs> WorkOrderLockedOrUnlocked;
    }
}