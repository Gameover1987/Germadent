using System;
using Germadent.Common.Logging;
using Germadent.Model.Production;
using Germadent.WebApi.DataAccess.Rma;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Germadent.WebApi.Controllers.Rms
{
    [ApiController]
    [Route("api/Rms/OrdersProcessing")]
    [Authorize]
    public class OrdersProcessingController : ControllerBase
    {
        private readonly IRmaDbOperations _rmaDbOperations;
        private readonly ILogger _logger;

        public OrdersProcessingController(IRmaDbOperations rmaDbOperations, ILogger logger)
        {
            _rmaDbOperations = rmaDbOperations;
            _logger = logger;
        }

        [HttpGet("GetWorksByWorkOrder/{workOrderId}/{userId}")]
        public IActionResult GetWorksByWorkOrder(int workOrderId, int userid)
        {
            try
            {
                _logger.Info(nameof(GetWorksByWorkOrder));
                var operations = _rmaDbOperations.GetWorksByWorkOrder(workOrderId, userid);
                return Ok(operations);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        [HttpGet("GetWorksInProgressByWorkOrder/{workOrderId}/{userId}")]
        public IActionResult GetWorksInProgressByWorkOrder(int workOrderId, int userid)
        {
            try
            {
                _logger.Info(nameof(GetWorksByWorkOrder));
                var operations = _rmaDbOperations.GetWorksInProgressByWorkOrder(workOrderId, userid);
                return Ok(operations);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        [HttpPost]
        [Route("StartWorks")]
        public IActionResult StartWorks(WorkDto[] works)
        {
            try
            {
                _logger.Info(nameof(StartWorks));
                _rmaDbOperations.StartWorks(works);
                return Ok();
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        [HttpPost]
        [Route("FinishWorks")]
        public IActionResult FinishWorks(WorkDto[] works)
        {
            try
            {
                _logger.Info(nameof(FinishWorks));
                _rmaDbOperations.FinishWorks(works);
                return Ok();
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }
    }
}
