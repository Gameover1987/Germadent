using Germadent.Common.Logging;
using Germadent.Model;
using Germadent.Model.Production;
using Germadent.WebApi.DataAccess.Rma;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Germadent.WebApi.Controllers.Rma
{
    [Route("api/Rma/Technology")]
    [ApiController]
    [Authorize]
    public class TechnologyController : CustomController
    {
        private readonly IRmaDbOperations _rmaDbOperations;

        public TechnologyController (IRmaDbOperations rmaDbOperations, ILogger logger, IHubContext<NotificationHub> hubContext)
            : base(logger, hubContext)
        {
            _rmaDbOperations = rmaDbOperations;
        }

        [HttpGet]
        [Route("EmployeePositions")]
        public IActionResult GetEmployeePositions()
        {
            return ExecuteAction(() => _rmaDbOperations.GetEmployeePositions());
        }

        [HttpGet]
        [Route("Operations")]
        public IActionResult GetTechnologyOperations()
        {
            return ExecuteAction(() => _rmaDbOperations.GetTechnologyOperations());
        }

        [HttpPost]
        [Route("AddOperation")]
        public IActionResult AddTechnologyOperation(TechnologyOperationDto technologyOperationDto)
        {
            return ExecuteRepositoryActionAndNotify(() => _rmaDbOperations.AddTechnologyOperation(technologyOperationDto), RepositoryType.TechnologyOperation, RepositoryAction.Add);
        }

        [HttpPost]
        [Route("UpdateOperation")]
        public IActionResult UpdateTechnologyOperation(TechnologyOperationDto technologyOperationDto)
        {
            return ExecuteRepositoryActionAndNotify(() => _rmaDbOperations.UpdateTechnologyOperation(technologyOperationDto), RepositoryType.TechnologyOperation, RepositoryAction.Update);
        }

        [HttpDelete]
        [Route("DeleteOperation/{technologyOperationId:int}")]
        public IActionResult DeleteTechnologyOperation(int technologyOperationId)
        {
            return ExecuteRepositoryActionAndNotify(() => _rmaDbOperations.DeleteTechnologyOperation(technologyOperationId), RepositoryType.TechnologyOperation, RepositoryAction.Delete);
        }
    }
}