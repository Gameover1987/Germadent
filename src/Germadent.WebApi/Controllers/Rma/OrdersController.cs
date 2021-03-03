using System;
using Germadent.Common.FileSystem;
using Germadent.Common.Logging;
using Germadent.Rma.Model;
using Germadent.WebApi.DataAccess;
using Germadent.WebApi.DataAccess.Rma;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;

namespace Germadent.WebApi.Controllers.Rma
{
    [ApiController]
    [Route("api/Rma/Orders")]
    [Authorize]
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
                _logger.Info(nameof(GetOrders));
                var orders = _rmaDbOperations.GetOrders(filter);
                return Ok(orders);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }
        
        [HttpGet("{workOrderId}/{userId}")]
        public IActionResult GetWorkOrderById(int workOrderId, int userId)
        {
            try
            {
                _logger.Info(nameof(GetWorkOrderById));
                var order = _rmaDbOperations.GetOrderDetails(workOrderId, userId);
                return Ok(order);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }
        
        [HttpPost]
        [Route("AddOrder")]
        public IActionResult AddOrder([FromBody] OrderDto orderDto)
        {
            try
            {
                _logger.Info(nameof(AddOrder));
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
        [Route("update")]
        public IActionResult UpdateOrder([FromBody] OrderDto orderDto)
        {
            try
            {
                _logger.Info(nameof(UpdateOrder));
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
                _logger.Info(nameof(CloseOrder));
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