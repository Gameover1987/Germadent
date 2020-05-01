using Germadent.Rma.Model;
using Germadent.WebApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Germadent.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IRmaDbOperations _rmaDbOperations;

        public CustomersController(IRmaDbOperations rmaDbOperations)
        {
            _rmaDbOperations = rmaDbOperations;
        }

        [HttpGet]
        public CustomerDto[] GetCustomers(string mask)
        {
            return _rmaDbOperations.GetCustomers(mask);
        }

        [HttpPost]
        public CustomerDto AddCustomer(CustomerDto customer)
        {
            return _rmaDbOperations.AddCustomer(customer);
        }
    }
}