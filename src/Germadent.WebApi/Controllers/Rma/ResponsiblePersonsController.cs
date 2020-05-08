using Germadent.Rma.Model;
using Germadent.WebApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Germadent.WebApi.Controllers.Rma
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponsiblePersonsController : ControllerBase
    {
        private readonly IRmaDbOperations _rmaDbOperations;

        public ResponsiblePersonsController(IRmaDbOperations rmaDbOperations)
        {
            _rmaDbOperations = rmaDbOperations;
        }

        [HttpGet]
        public CustomerDto[] GetResponsiblePersons()
        {
            return null;
            //return _rmaDbOperations.GetResponsiblePersons();
        }

        [HttpPost]
        public CustomerDto AddResponsiblePerson(CustomerDto customer)
        {
            return null;
            //return _rmaDbOperations.add(customer);
        }
    }
}