using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Germadent.Rma.Model;
using Germadent.WebApi.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Germadent.WebApi.Controllers
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