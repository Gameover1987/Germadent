using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Germadent.Common.Extensions;
using Germadent.Common.Logging;
using Germadent.Rma.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Germadent.WebApi.Controllers
{
    public enum RepositoryAction
    {
        None, Add, Update, Delete
    }

    public class CustomController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public CustomController(ILogger logger, IHubContext<NotificationHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }

        protected IActionResult ExecuteRepositoryActionAndNotify<T>(Func<T> func, RepositoryType repositoryType, RepositoryAction repositoryAction)
        {
            try
            {
                _logger.Info(string.Format("Method: {0}, API: {1}", HttpContext.Request.Method, HttpContext.Request.Path.Value));
                var result = func();

                switch (repositoryAction)
                {
                    case RepositoryAction.Add:
                        SendAddNotification(repositoryType, result);
                        break;
                    case RepositoryAction.Update:
                        SendUpdateNotification(repositoryType, result);
                        break;
                    case RepositoryAction.Delete:
                        var identityDto = (IIdentityDto)result;
                        SendDeleteNotification(repositoryType, identityDto.GetIdentificator());
                        break;
                }

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        protected IActionResult ExecuteAction<T>(Func<T> func)
        {
            try
            {
                _logger.Info(string.Format("Method: {0}, API: {1}", HttpContext.Request.Method, HttpContext.Request.Path.Value));
                var result = func();

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        protected void SendUpdateNotification(RepositoryType repositoryType, object updatedItem)
        {
            var repositoryNotificationDto = new RepositoryNotificationDto(repositoryType);
            if (updatedItem != null)
                repositoryNotificationDto.ChangedItems = new[] { updatedItem };
            _hubContext.Clients.All.SendAsync("Send", repositoryNotificationDto.SerializeToJson());
        }

        protected void SendAddNotification(RepositoryType repositoryType, object addedItem)
        {
            var repositoryNotificationDto = new RepositoryNotificationDto(repositoryType);
            if (addedItem != null)
                repositoryNotificationDto.AddedItems = new[] { addedItem };
            _hubContext.Clients.All.SendAsync("Send", repositoryNotificationDto.SerializeToJson());
        }

        protected void SendDeleteNotification(RepositoryType repositoryType, int deletedId)
        {
            var repositoryNotificationDto = new RepositoryNotificationDto(repositoryType);
            repositoryNotificationDto.DeletedItems = new[] { deletedId };
            _hubContext.Clients.All.SendAsync("Send", repositoryNotificationDto.SerializeToJson());
        }
    }
}
