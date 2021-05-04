using System;
using Germadent.Common.Extensions;
using Germadent.Common.Logging;
using Germadent.Rma.Model;
using Germadent.Rma.Model.Pricing;
using Germadent.Rma.Model.Production;
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
            var result = ExecuteRepositoryActionAndNotify(() => _rmaDbOperations.AddPricePosition(pricePositionDto), RepositoryType.PricePosition, RepositoryAction.Add);
            SendAddNotification(RepositoryType.Product, null);
            return result;
        }

        [HttpPost]
        [Route("UpdatePricePosition")]
        public IActionResult UpdatePricePosition(PricePositionDto pricePositionDto)
        {
            var result = ExecuteRepositoryActionAndNotify(() => _rmaDbOperations.UpdatePricePosition(pricePositionDto), RepositoryType.PricePosition, RepositoryAction.Update);
            SendUpdateNotification(RepositoryType.Product, null);
            return result;
        }

        [HttpDelete]
        [Route("DeletePricePosition/{pricePositionId:int}")]
        public IActionResult DeletePricePosition(int pricePositionId)
        {
            var result = ExecuteRepositoryActionAndNotify(() => _rmaDbOperations.DeletePricePosition(pricePositionId), RepositoryType.PricePosition, RepositoryAction.Delete);
            SendDeleteNotification(RepositoryType.Product, pricePositionId);
            return result;
        }

        [HttpGet]
        [Route("GetProducts")]
        public IActionResult GetProducts()
        {
            return ExecuteAction(() => _rmaDbOperations.GetProducts());
        }
    }
}
