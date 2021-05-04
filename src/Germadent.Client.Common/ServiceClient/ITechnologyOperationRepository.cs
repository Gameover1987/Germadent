using Germadent.Model.Production;

namespace Germadent.Client.Common.ServiceClient
{
    public interface ITechnologyOperationRepository : IRepository<TechnologyOperationDto>
    {
        public void AddTechnologyOperation(TechnologyOperationDto technologyOperationDto);

        public void EditTechnologyOperation(TechnologyOperationDto technologyOperationDto);

        public void DeleteTechnologyOperation(int technologyOperationId);
    }
}