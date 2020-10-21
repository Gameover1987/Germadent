using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Germadent.Common.Logging;
using Germadent.UserManagementCenter.Model.Rights;
using Germadent.WebApi.DataAccess.UserManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Germadent.WebApi.Controllers.UserManagement
{
    [Route("api/userManagement/authorization")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IUmcDbOperations _umcDbOperations;
        private readonly ILogger _logger;

        public AuthorizationController(IUmcDbOperations umcDbOperations, ILogger logger)
        {
            _umcDbOperations = umcDbOperations;
            _logger = logger;
        }

        [HttpGet]
        [Route("Authorize/{login}/{password}")]
        public IActionResult Authorize(string login, string password)
        {
            try
            {
                _logger.Info(nameof(Authorize));
                var authorizationInfoDto = _umcDbOperations.Authorize(login, password);
                return Ok(authorizationInfoDto);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }
    }
}
