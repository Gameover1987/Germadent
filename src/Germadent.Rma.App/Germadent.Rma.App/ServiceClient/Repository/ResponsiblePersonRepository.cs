using System;
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

        public ResponsiblePersonRepository(IRmaServiceClient rmaServiceClient)
        {
            _rmaServiceClient = rmaServiceClient;
            _rmaServiceClient.ResponsiblePersonRepositoryChanged += RmaServiceClientOnResponsiblePersonRepositoryChanged;
        }

        private void RmaServiceClientOnResponsiblePersonRepositoryChanged(object sender, ResponsiblePersonRepositoryChangedEventArgs e)
        {
            ReLoad();
        }

        protected override ResponsiblePersonDto[] GetItems()
        {
            return _rmaServiceClient.GetResponsiblePersons().ToArray();
        }
    }
}