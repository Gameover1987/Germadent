using Germadent.Common.Logging;
using Germadent.Rma.Model;
using Germadent.WebApi.DataAccess.Rma;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Germadent.WebApi.Controllers.Rma
{
    [ApiController]
    [Route("api/Rma/Customers")]
    [Authorize]
    public class CustomersController : CustomController
    {
        private readonly IRmaDbOperations _rmaDbOperations;

        public CustomersController(IRmaDbOperations rmaDbOperations, ILogger logger, IHubContext<NotificationHub> hubContext) 
            : base(logger, hubContext)
        {
            _rmaDbOperations = rmaDbOperations;
        }

        [HttpGet]
        //[Authorize]
        public IActionResult GetCustomers(string mask)
        {
            return ExecuteAction(() => _rmaDbOperations.GetCustomers(mask));
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddCustomer(CustomerDto customer)
        {
            return ExecuteRepositoryActionAndNotify(() => _rmaDbOperations.AddCustomer(customer), RepositoryType.Customer, RepositoryAction.Add);
        }

        [HttpPost]
        [Route("update")]
        public IActionResult UpdateCustomer(CustomerDto customer)
        {
            return ExecuteRepositoryActionAndNotify(() => _rmaDbOperations.UpdateCustomer(customer), RepositoryType.Customer, RepositoryAction.Update);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteCustomer(int id)
        {
            return ExecuteRepositoryActionAndNotify(() => _rmaDbOperations.DeleteCustomer(id), RepositoryType.Customer, RepositoryAction.Delete);
        }
    }
}