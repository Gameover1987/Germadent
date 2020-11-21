using System.Linq;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.ServiceClient.Repository
{
    public interface IResponsiblePersonRepository : IRepository<ResponsiblePersonDto>
    {

    }

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
            ReLoad();
        }

        protected override ResponsiblePersonDto[] GetItems()
        {
            return _rmaServiceClient.GetResponsiblePersons().ToArray();
        }
    }
}