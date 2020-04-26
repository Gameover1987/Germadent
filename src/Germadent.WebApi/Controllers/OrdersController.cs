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

        [HttpGet("{id}")]
        public OrderDto GetWorkOrderById(int id)
        {
            return _rmaDbOperations.GetOrderDetails(id);
        }

        [HttpGet]
        public OrderLiteDto[] GetOrders()
        {
            return _rmaDbOperations.GetOrders(new OrdersFilter());
        }
    }
}