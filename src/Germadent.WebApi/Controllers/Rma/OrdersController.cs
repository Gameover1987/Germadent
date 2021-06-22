using System;
using Germadent.Common.Extensions;
using Germadent.Common.Logging;
using Germadent.Model;
using Germadent.WebApi.DataAccess.Rma;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Germadent.WebApi.Controllers.Rma
{
    [ApiController]
    [Route("api/Rma/Orders")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IRmaDbOperations _rmaDbOperations;
        private readonly ILogger _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public OrdersController(IRmaDbOperations rmaDbOperations, ILogger logger, IHubContext<NotificationHub> hubContext)
        {
            _rmaDbOperations = rmaDbOperations;
            _logger = logger;
            _hubContext = hubContext;
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

                var lockDto = new OrderLockInfoDto
                {
                    WorkOrderId = workOrderId,
                    OccupancyDateTime = order.LockDate,
                    User = order.LockedBy,
                    IsLocked = true
                };

                _hubContext.Clients.All.SendAsync("LockOrUnlock", lockDto.SerializeToJson());
                return Ok(order);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        [HttpGet("UnlockWorkOrder/{workOrderId}")]
        public IActionResult UnlockWorkOrder(int workOrderId)
        {
            try
            {
                _logger.Info(nameof(UnlockWorkOrder));
                _rmaDbOperations.UnlockWorkOrder(workOrderId);

                var lockInfo = new OrderLockInfoDto
                {
                    WorkOrderId = workOrderId,
                    IsLocked = false
                };

                _hubContext.Clients.All.SendAsync("LockOrUnlock", lockInfo.SerializeToJson());
                return Ok(lockInfo);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }


        [HttpPost]
        [Route("add")]
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

        [HttpGet]
        [Route("CloseOrder/{workOrderId}/{userId}")]
        public IActionResult CloseOrder(int workOrderId, int userId)
        {
            try
            {
                _logger.Info(nameof(CloseOrder));
                var notificationDto = _rmaDbOperations.CloseOrder(workOrderId, userId);
                if (notificationDto != null)
                    _hubContext.Clients.All.SendAsync("OrderStatusChanged", notificationDto.SerializeToJson());
                return Ok();
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        [HttpGet]
        [Route("GetWorksByWorkOrder/{workOrderId}")]
        public IActionResult GetWorksByWorkOrder(int workOrderId)
        {
            try
            {
                _logger.Info(nameof(GetWorksByWorkOrder));
                var works = _rmaDbOperations.GetAllWorksByWorkOrder(workOrderId);
                return Ok(works);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }
    }
}