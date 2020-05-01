using System.IO;
using System.Linq;
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

        [HttpGet("{id:int}")]
        public OrderDto GetWorkOrderById(int id)
        {
            return _rmaDbOperations.GetOrderDetails(id);
        }


        [HttpPost]

        public OrderDto AddOrder([FromForm] OrderDto orderDto)
        {
            Stream stream = null;
            if (Request.Form != null && Request.Form.Files.Any())
            {
                stream = Request.Form.Files.GetFile("DataFile").OpenReadStream();
            }

            return _rmaDbOperations.AddOrder(orderDto, stream);

        }
    }
}