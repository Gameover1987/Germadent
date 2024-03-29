﻿using System.Linq;
using Germadent.Client.Common.ServiceClient;
using Germadent.Client.Common.ServiceClient.Notifications;
using Germadent.Client.Common.ServiceClient.Repository;
using Germadent.Model;

namespace Germadent.Rma.App.ServiceClient.Repository
{
    public class ResponsiblePersonRepository : Repository<ResponsiblePersonDto>, IResponsiblePersonRepository
    {
        private readonly IRmaServiceClient _rmaServiceClient;
        private readonly ISignalRClient _signalRClient;

        public ResponsiblePersonRepository(IRmaServiceClient rmaServiceClient, ISignalRClient signalRClient)
        {
            _rmaServiceClient = rmaServiceClient;
            _signalRClient = signalRClient;
            _signalRClient.ResponsiblePersonRepositoryChanged += SignalRClientOnResponsiblePersonRepositoryChanged;
        }

        private void SignalRClientOnResponsiblePersonRepositoryChanged(object sender, RepositoryChangedEventArgs<ResponsiblePersonDto> e)
        {
            OnRepositoryChanged(this, e);
        }

        protected override ResponsiblePersonDto[] GetItems()
        {
            return _rmaServiceClient.GetResponsiblePersons().ToArray();
        }
    }
}