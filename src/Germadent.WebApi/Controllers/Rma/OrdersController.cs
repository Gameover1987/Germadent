using System;
using Germadent.Common.FileSystem;
using Germadent.Common.Logging;
using Germadent.Rma.Model;
using Germadent.WebApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Germadent.WebApi.Controllers.Rma
{
    [Route("api/Rma/Orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IRmaDbOperations _rmaDbOperations;
        private readonly IFileManager _fileManager;
        private readonly ILogger _logger;

        public OrdersController(IRmaDbOperations rmaDbOperations, IFileManager fileManager, ILogger logger)
        {
            _rmaDbOperations = rmaDbOperations;
            _fileManager = fileManager;
            _logger = logger;
        }

        [HttpPost]
        [Route("getByFilter")]
        public IActionResult GetOrders(OrdersFilter filter)
        {
            try
            {
                _logger.Info("Getting orders by filter");
                var orders = _rmaDbOperations.GetOrders(filter);
                return Ok(orders);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult GetWorkOrderById(int id)
        {
            try
            {
                var order = _rmaDbOperations.GetOrderDetails(id);
                return Ok(order);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }


        [HttpPost]
        public IActionResult AddOrder([FromBody] OrderDto orderDto)
        {
            try
            {
                var order = _rmaDbOperations.AddOrder(orderDto);
                return Ok(order);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        [HttpPost]
        [Route("fileUpload/{id}/{fileName}")]
        public IActionResult FileUpload(int id, string fileName)
        {
            try
            {
                var stream = Request.Form.Files.GetFile("DataFile").OpenReadStream();
                _rmaDbOperations.AttachDataFileToOrder(id, fileName, stream);
                return Ok();
            }
            catch(Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        [HttpGet]
        [Route("fileDownload/{id}")]
        public IActionResult FileDownload(int id)
        {
            try
            {
                var fullFileName = _rmaDbOperations.GetFileByWorkOrder(id);
                if (fullFileName == null)
                    return null;

                var stream = _fileManager.OpenFileAsStream(fullFileName);

                var fileStreamResult = new FileStreamResult(stream, "application/octet-stream");
                return fileStreamResult;
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        [HttpPut]
        public IActionResult UpdateOrder([FromBody] OrderDto orderDto)
        {
            try
            {
                _rmaDbOperations.UpdateOrder(orderDto);
                return Ok(orderDto);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult CloseOrder(int id)
        {
            try
            {
                var order = _rmaDbOperations.CloseOrder(id);
                return Ok(order);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }
    }
}