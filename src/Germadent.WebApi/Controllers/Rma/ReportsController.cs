﻿using System;
using Germadent.Common.Logging;
using Germadent.WebApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Germadent.WebApi.Controllers.Rma
{
    [Route("api/Rma/Reports")]
    [ApiController]
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