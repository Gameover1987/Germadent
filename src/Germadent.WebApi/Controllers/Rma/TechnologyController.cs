using Germadent.Common.Logging;
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
    }
}