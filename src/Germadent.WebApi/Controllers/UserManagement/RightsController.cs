using System;
using Germadent.Common.Logging;
using Germadent.WebApi.DataAccess;
using Germadent.WebApi.DataAccess.UserManagement;
using Microsoft.AspNetCore.Mvc;

namespace Germadent.WebApi.Controllers.UserManagement
{
    [Route("api/userManagement/[controller]")]
    [ApiController]
    public class RightsController : ControllerBase
    {
        private readonly IUmcDbOperations _umcDbOperations;
        private readonly ILogger _logger;

        public RightsController(IUmcDbOperations umcDbOperations, ILogger logger)
        {
            _umcDbOperations = umcDbOperations;
            _logger = logger;
        }


        [HttpGet]
        public IActionResult GetRights()
        {
            try
            {
                _logger.Info(nameof(GetRights));
                var rights = _umcDbOperations.GetRights();
                return Ok(rights);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }
    }
}