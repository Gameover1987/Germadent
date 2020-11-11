using System;
using Germadent.Common.Extensions;
using Germadent.Common.Logging;
using Germadent.Rma.Model;
using Germadent.Rma.Model.Pricing;
using Germadent.WebApi.DataAccess.Rma;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Germadent.WebApi.Controllers.Rma
{
    [Route("api/Rma/Pricing")]
    [ApiController]
    public class PriceController : ControllerBase
    {
        private readonly IRmaDbOperations _rmaDbOperations;
        private readonly ILogger _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public PriceController(IRmaDbOperations rmaDbOperations, ILogger logger, IHubContext<NotificationHub> hubContext)
        {
            _rmaDbOperations = rmaDbOperations;
            _logger = logger;
            _hubContext = hubContext;
        }

        [HttpGet]
        [Route("PriceGroups/{branchType:int}")]
        public IActionResult GetPriceGroups(int branchType)
        {
            try
            {
                _logger.Info(nameof(GetPriceGroups));
                var priceGroups = _rmaDbOperations.GetPriceGroups((BranchType)branchType);
                return Ok(priceGroups);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        [HttpPost]
        [Route("AddPriceGroup")]
        public IActionResult AddPriceGroup(PriceGroupDto priceGroup)
        {
            try
            {
                _logger.Info(nameof(AddPriceGroup));
                priceGroup = _rmaDbOperations.AddPriceGroup(priceGroup);

                var repositoryNotificationDto = new RepositoryNotificationDto(RepositoryType.PriceGroup);
                repositoryNotificationDto.AddedItems = new[] { priceGroup };
                _hubContext.Clients.All.SendAsync("Send", repositoryNotificationDto.SerializeToJson());

                return Ok(priceGroup);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        [HttpPost]
        [Route("UpdatePriceGroup")]
        public IActionResult UpdatePriceGroup(PriceGroupDto priceGroup)
        {
            try
            {
                _logger.Info(nameof(UpdatePriceGroup));
                priceGroup = _rmaDbOperations.UpdatePriceGroup(priceGroup);

                var repositoryNotificationDto = new RepositoryNotificationDto(RepositoryType.PriceGroup);
                repositoryNotificationDto.ChangedItems = new[] { priceGroup };
                _hubContext.Clients.All.SendAsync("Send", repositoryNotificationDto.SerializeToJson());

                return Ok(priceGroup);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        [HttpDelete]
        [Route("DeletePriceGroup/{priceGroupId:int}")]
        public IActionResult DeletePriceGroup(int priceGroupId)
        {
            try
            {
                _logger.Info(nameof(DeletePriceGroup));
                var result = _rmaDbOperations.DeletePriceGroup(priceGroupId);

                var repositoryNotificationDto = new RepositoryNotificationDto(RepositoryType.PriceGroup);
                repositoryNotificationDto.DeletedItems = new[] {priceGroupId};
                _hubContext.Clients.All.SendAsync("Send", repositoryNotificationDto.SerializeToJson());

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        [HttpGet]
        [Route("PricePositions/{branchType:int}")]
        public IActionResult GetPricePositions(int branchType)
        {
            try
            {
                _logger.Info(nameof(GetPricePositions));
                var priceGroups = _rmaDbOperations.GetPricePositions((BranchType)branchType);
                return Ok(priceGroups);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        [HttpPost]
        [Route("AddPricePosition")]
        public IActionResult AddPricePosition(PricePositionDto pricePositionDto)
        {
            try
            {
                _logger.Info(nameof(AddPricePosition));
                pricePositionDto = _rmaDbOperations.AddPricePosition(pricePositionDto);
                return Ok(pricePositionDto);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        [HttpPost]
        [Route("UpdatePricePosition")]
        public IActionResult UpdatePricePosition(PricePositionDto pricePositionDto)
        {
            try
            {
                _logger.Info(nameof(UpdatePricePosition));
                pricePositionDto = _rmaDbOperations.UpdatePricePosition(pricePositionDto);
                return Ok(pricePositionDto);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        [HttpDelete]
        [Route("DeletePricePosition/{pricePositionId:int}")]
        public IActionResult DeletePricePosition(int pricePositionId)
        {
            try
            {
                _logger.Info(nameof(DeletePricePosition));
                var result = _rmaDbOperations.DeletePricePosition(pricePositionId);
                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }
    }
}
