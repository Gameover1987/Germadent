using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.Client.Common.Infrastructure;
using Germadent.Client.Common.ServiceClient.Notifications;
using Germadent.Client.Common.ServiceClient.Repository;
using Germadent.Client.Common.ViewModels;
using Germadent.Common.Logging;
using Germadent.Model;
using Germadent.Model.Pricing;
using Germadent.Model.Production;
using Germadent.Rms.App.ServiceClient;
using Germadent.Rms.App.ViewModels;
using Germadent.UI.Commands;
using Germadent.UI.Infrastructure;
using Germadent.UI.ViewModels.DesignTime;

namespace Germadent.Rms.App.Views.DesignMock
{
    public class DesignMockWpfEnvironment : IEnvironment
    {
        public void Restart()
        {
            throw new NotImplementedException();
        }

        public void Shutdown()
        {
            throw new NotImplementedException();
        }
    }

    public class DesignMockRmsUserManager : IUserManager
    {
        public bool HasRight(string rightName)
        {
            throw new NotImplementedException();
        }

        public AuthorizationInfoDto AuthorizationInfo { get; }
    }

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

    public class DesignMockMainViewModel : MainViewModel
    {
        public DesignMockMainViewModel() 
            : base(new DesignMockLogger(),
                new DesignMockRmsServiceClient(), 
                new DesignMockWpfEnvironment(), 
                new DesignMockRmsUserManager(), 
                new DesignMockShowDialogAgent(), 
                new DesignMockOrdersFilterViewModel(), 
                new DesignMockStartWorkListViewModelViewModel(), 
                new DesignMockFinishWorkListViewModel(), 
                new DesignMockSignalRClient())
        {
        }
    }
}
