using Germadent.Client.Common.ServiceClient;
using Germadent.Client.Common.ServiceClient.Notifications;
using Germadent.Client.Common.ServiceClient.Repository;
using Germadent.Model.Production;

namespace Germadent.Rma.App.ServiceClient.Repository
{
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
            OnRepositoryChanged(this, e);
        }

        protected override TechnologyOperationDto[] GetItems()
        {
            return _rmaServiceClient.GetTechnologyOperations();
        }

        public void AddTechnologyOperation(TechnologyOperationDto technologyOperationDto)
        {
            _rmaServiceClient.AddTechnologyOperation(technologyOperationDto);
        }

        public void EditTechnologyOperation(TechnologyOperationDto technologyOperationDto)
        {
            _rmaServiceClient.UpdateTechnologyOperation(technologyOperationDto);
        }

        public void DeleteTechnologyOperation(int technologyOperationId)
        {
            _rmaServiceClient.DeleteTechnologyOperation(technologyOperationId);
        }
    }
}