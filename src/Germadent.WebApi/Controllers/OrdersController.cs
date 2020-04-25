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
    public class OrdersController : ControllerBase
    {
        private readonly IRmaDbOperations _rmaDbOperations;

        public OrdersController(IRmaDbOperations rmaDbOperations)
        {
            _rmaDbOperations = rmaDbOperations;
        }

        [HttpGet]
        public OrderDto[] Get()
        {
            return new OrderDto[]
            {
                new OrderDto(),
            };
        }
    }
}