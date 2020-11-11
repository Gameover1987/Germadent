using System;
using Germadent.Common.Logging;
using Germadent.WebApi.DataAccess.UserManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Germadent.WebApi.Controllers.UserManagement
{
    [Route("api/userManagement/authorization")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IUmcDbOperations _umcDbOperations;
        private readonly ILogger _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public AuthorizationController(IUmcDbOperations umcDbOperations, ILogger logger, IHubContext<NotificationHub> hubContext)
        {
            _umcDbOperations = umcDbOperations;
            _logger = logger;
            _hubContext = hubContext;
        }

        [HttpGet]
        [Route("Authorize/{login}/{password}")]
        public IActionResult Authorize(string login, string password)
        {
            try
            {
                _logger.Info(nameof(Authorize));
                var authorizationInfoDto = _umcDbOperations.Authorize(login, password);

                _hubContext.Clients.All.SendAsync("Send", "Preved from SignalR!","Param2");

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
