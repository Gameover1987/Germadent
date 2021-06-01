using System;
using Germadent.Common.Logging;
using Germadent.Model;
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

        [HttpPost]
        [Route("GetSalaryReport")]
        public IActionResult GetSalaryReport(SalaryFilter salaryFilter)
        {
            try
            {
                _logger.Info(nameof(GetSalaryReport));
                return Ok(_rmaDbOperations.GetSalaryReport(salaryFilter));
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }
    }
}