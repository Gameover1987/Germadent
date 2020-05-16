using Germadent.Rma.Model;
using Germadent.WebApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Germadent.WebApi.Controllers.Rma
{
    [Route("api/rma/responsiblePersons")]
    [ApiController]
    public class ResponsiblePersonsController : ControllerBase
    {
        private readonly IRmaDbOperations _rmaDbOperations;

        public ResponsiblePersonsController(IRmaDbOperations rmaDbOperations)
        {
            _rmaDbOperations = rmaDbOperations;
        }

        [HttpGet]
        public ResponsiblePersonDto[] GetResponsiblePersons()
        {
            return _rmaDbOperations.GetResponsiblePersons();
        }

        [HttpPost]
        public ResponsiblePersonDto AddResponsiblePerson(ResponsiblePersonDto responsiblePerson)
        {
            return _rmaDbOperations.AddResponsiblePerson(responsiblePerson);
        }

        [HttpPut]
        public ResponsiblePersonDto UpdateResponsiblePerson(ResponsiblePersonDto responsiblePerson)
        {
            return _rmaDbOperations.UpdateResponsiblePerson(responsiblePerson);
        }

        [HttpDelete("{id:int}")]
        public ResponsiblePersonDeleteResult DeleteResponsiblePerson(int id)
        {
            return _rmaDbOperations.DeleteResponsiblePerson(id);
        }
    }
}