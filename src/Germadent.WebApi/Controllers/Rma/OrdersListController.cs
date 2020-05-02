using Germadent.Rma.Model;
using Germadent.WebApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Germadent.WebApi.Controllers.Rma
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersListController : ControllerBase
    {
        private readonly IRmaDbOperations _rmaDbOperations;

        public OrdersListController(IRmaDbOperations rmaDbOperations)
        {
            _rmaDbOperations = rmaDbOperations;
        }

        [HttpPost]
        public OrderLiteDto[] GetOrders(OrdersFilter filter)
        {
            return _rmaDbOperations.GetOrders(filter);
        }
    }
}