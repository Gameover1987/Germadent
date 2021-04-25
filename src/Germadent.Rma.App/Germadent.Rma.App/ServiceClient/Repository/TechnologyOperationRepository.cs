using Germadent.Rma.Model.Production;

namespace Germadent.Rma.App.ServiceClient.Repository
{
    public interface ITechnologyOperationRepository : IRepository<TechnologyOperationDto>
    {
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
            ReLoad();
        }

        protected override TechnologyOperationDto[] GetItems()
        {
            return _rmaServiceClient.GetTechnologyOperations();
        }

        public void DeleteTechnologyOperation(int technologyOperationId)
        {
            _rmaServiceClient.DeleteTechnologyOperation(technologyOperationId);
        }
    }
}