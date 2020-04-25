using Germadent.Rma.Model;
using Germadent.WebApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Germadent.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IRmaDbOperations _rmaDbOperations;

        public OrdersController(IRmaDbOperations rmaDbOperations)
        {
            _rmaDbOperations = rmaDbOperations;
        }

        [HttpGet]
        public OrderLiteDto[] Get()
        {
            return _rmaDbOperations.GetOrders(new OrdersFilter());
        }
    }
}