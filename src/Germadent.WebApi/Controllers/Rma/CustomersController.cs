using Germadent.Rma.Model;
using Germadent.WebApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Germadent.WebApi.Controllers.Rma
{
    [Route("api/Rma/Customers")]
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

        [HttpPut]
        public CustomerDto UpdateCustomer(CustomerDto customer)
        {
            return _rmaDbOperations.UpdateCustomer(customer);
        }

        [HttpDelete("{id:int}")]
        public CustomerDeleteResult DeleteCustomer(int id)
        {
            return _rmaDbOperations.DeleteCustomer(id);
        }
    }
}