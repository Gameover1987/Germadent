using System;
using Germadent.Common.Extensions;
using Germadent.Common.Logging;
using Germadent.Rma.Model;
using Germadent.Rma.Model.Pricing;
using Germadent.WebApi.DataAccess.Rma;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Germadent.WebApi.Controllers.Rma
{
    [Route("api/Rma/Pricing")]
    [ApiController]
    [Authorize]
    public class PriceController : CustomController
    {
        private readonly IRmaDbOperations _rmaDbOperations;

        public PriceController(IRmaDbOperations rmaDbOperations, ILogger logger, IHubContext<NotificationHub> hubContext)
            : base(logger, hubContext)
        {
            _rmaDbOperations = rmaDbOperations;
        }

        [HttpGet]
        [Route("PriceGroups/{branchType:int}")]
        public IActionResult GetPriceGroups(int branchType)
        {
            return ExecuteAction(() => _rmaDbOperations.GetPriceGroups((BranchType)branchType));
        }

        [HttpPost]
        [Route("AddPriceGroup")]
        public IActionResult AddPriceGroup(PriceGroupDto priceGroup)
        {
            return ExecuteRepositoryActionAndNotify(() => _rmaDbOperations.AddPriceGroup(priceGroup), RepositoryType.PriceGroup, RepositoryAction.Add);
        }

        [HttpPost]
        [Route("UpdatePriceGroup")]
        public IActionResult UpdatePriceGroup(PriceGroupDto priceGroup)
        {
            return ExecuteRepositoryActionAndNotify(() => _rmaDbOperations.UpdatePriceGroup(priceGroup), RepositoryType.PriceGroup, RepositoryAction.Update);
        }

        [HttpDelete]
        [Route("DeletePriceGroup/{priceGroupId:int}")]
        public IActionResult DeletePriceGroup(int priceGroupId)
        {
            return ExecuteRepositoryActionAndNotify(() => _rmaDbOperations.DeletePriceGroup(priceGroupId), RepositoryType.PriceGroup, RepositoryAction.Delete);
        }

        [HttpGet]
        [Route("PricePositions/{branchType:int}")]
        public IActionResult GetPricePositions(int branchType)
        {
            return ExecuteAction(() => _rmaDbOperations.GetPricePositions((BranchType)branchType));
        }

        [HttpPost]
        [Route("AddPricePosition")]
        public IActionResult AddPricePosition(PricePositionDto pricePositionDto)
        {
            return ExecuteRepositoryActionAndNotify(() => _rmaDbOperations.AddPricePosition(pricePositionDto), RepositoryType.PricePosition, RepositoryAction.Add);
        }

        [HttpPost]
        [Route("UpdatePricePosition")]
        public IActionResult UpdatePricePosition(PricePositionDto pricePositionDto)
        {
            return ExecuteRepositoryActionAndNotify(() => _rmaDbOperations.UpdatePricePosition(pricePositionDto), RepositoryType.PricePosition, RepositoryAction.Update);
        }

        [HttpDelete]
        [Route("DeletePricePosition/{pricePositionId:int}")]
        public IActionResult DeletePricePosition(int pricePositionId)
        {
            return ExecuteRepositoryActionAndNotify(() => _rmaDbOperations.DeletePricePosition(pricePositionId), RepositoryType.PricePosition, RepositoryAction.Delete);
        }

        [HttpGet]
        [Route("GetProducts/{branchType:int}")]
        public IActionResult GetProductSetForPriceGroup(int branchType)
        {
            return ExecuteAction(() => _rmaDbOperations.GetProductSetForToothCard((BranchType)branchType));
        }
    }
}
