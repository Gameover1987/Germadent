using System;
using Germadent.Rma.Model.Production;

namespace Germadent.Rma.App.ServiceClient.Repository
{
    public interface ITechnologyOperationRepository : IRepository<TechnologyOperationDto>
    {
        event EventHandler<RepositoryChangedEventArgs<TechnologyOperationDto>> ChangedNew;

        public void DeleteTechnologyOperation(int technologyOperationId);
    }

    public class TechnologyOperationRepository : Repository<TechnologyOperationDto>, ITechnologyOperationRepository
    {
        private readonly IRmaServiceClient _rmaServiceClient;
        private readonly ISignalRClient _signalRClient;

        public TechnologyOperationRepository(IRmaServiceClient rmaServiceClient, ISignalRClient signalRClient)
        {
            _rmaServiceClient = rmaServiceClient;
            _signalRClient = signalRClient;
            _signalRClient.TechnologyOperationRepositoryChanged += SignalRClientOnTechnologyOperationRepositoryChanged;
        }

        private void SignalRClientOnTechnologyOperationRepositoryChanged(object sender, RepositoryChangedEventArgs<TechnologyOperationDto> e)
        {
            ChangedNew?.Invoke(this, e);
        }

        protected override TechnologyOperationDto[] GetItems()
        {
            return _rmaServiceClient.GetTechnologyOperations();
        }

        public event EventHandler<RepositoryChangedEventArgs<TechnologyOperationDto>> ChangedNew;

        public void DeleteTechnologyOperation(int technologyOperationId)
        {
            _rmaServiceClient.DeleteTechnologyOperation(technologyOperationId);
        }
    }
}