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
            return null;
            //return _rmaDbOperations.add(customer);
        }
    }
}