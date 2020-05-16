using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Germadent.Rma.Model;
using Germadent.WebApi.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Germadent.WebApi.Controllers.Rma
{
    [Route("api/rma/reports")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IRmaDbOperations _rmaDbOperations;

        public ReportsController(IRmaDbOperations rmaDbOperations)
        {
            _rmaDbOperations = rmaDbOperations;
        }

        [HttpGet("{id:int}")]
        public ReportListDto[] GetReports(int id)
        {
            return _rmaDbOperations.GetWorkReport(id);
        }
    }
}