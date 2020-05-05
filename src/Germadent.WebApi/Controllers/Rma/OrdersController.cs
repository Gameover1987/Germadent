using System.IO;
using Germadent.Rma.Model;
using Germadent.WebApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Germadent.WebApi.Controllers.Rma
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
        public OrderDto AddOrder([FromBody] OrderDto orderDto)
        {
            Stream stream = null;
            //if (Request.ContentType.Contains("multipart/form-data") && Request.Form.Files.Any())
            //{
            //    stream = Request.Form.Files.GetFile("DataFile").OpenReadStream();
            //}

            return _rmaDbOperations.AddOrder(orderDto, stream);
        }

        [HttpPut]
        public OrderDto UpdateOrder([FromBody] OrderDto orderDto)
        {
            Stream stream = null;

            _rmaDbOperations.UpdateOrder(orderDto, stream);

            return orderDto;
        }

        [HttpDelete("{id:int}")]
        public OrderDto CloseOrder(int id)
        {
            return _rmaDbOperations.CloseOrder(id);
        }
    }
}