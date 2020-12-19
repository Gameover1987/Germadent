using Germadent.Common.Logging;
using Germadent.WebApi.DataAccess.Rma;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Germadent.WebApi.Controllers.Rma
{
    [ApiController]
    [Route("api/Rma/Attributes")]
    [Authorize]
    public class AttributesController : CustomController
    {
        private readonly IRmaDbOperations _rmaDbOperations;

        public AttributesController(ILogger logger, IHubContext<NotificationHub> hubContext, IRmaDbOperations rmaDbOperations) 
            : base(logger, hubContext)
        {
            _rmaDbOperations = rmaDbOperations;
        }

        [HttpGet]
        [Route("GetAttributesAndValues")]
        public IActionResult GetAttributesAndValues()
        {
            return ExecuteAction(() => _rmaDbOperations.GetAllAttributesAndValues());
        }
    }
}