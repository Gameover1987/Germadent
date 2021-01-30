using System;
using Germadent.Common.Logging;
using Germadent.WebApi.DataAccess;
using Germadent.WebApi.DataAccess.Rma;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Germadent.WebApi.Controllers.Rma
{
    [Route("api/Rma/Reports")]
    [ApiController]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IRmaDbOperations _rmaDbOperations;
        private readonly ILogger _logger;

        public ReportsController(IRmaDbOperations rmaDbOperations, ILogger logger)
        {
            _rmaDbOperations = rmaDbOperations;
            _logger = logger;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetReports(int id)
        {
            try
            {
                _logger.Info(nameof(GetReports));
                return Ok(_rmaDbOperations.GetWorkReport(id));
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }
    }
}