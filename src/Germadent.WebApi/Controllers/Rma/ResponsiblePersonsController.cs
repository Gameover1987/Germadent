using System;
using Germadent.Common.Logging;
using Germadent.Rma.Model;
using Germadent.WebApi.DataAccess;
using Germadent.WebApi.DataAccess.Rma;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Germadent.WebApi.Controllers.Rma
{
    [Route("api/Rma/ResponsiblePersons")]
    [ApiController]
    public class ResponsiblePersonsController : CustomController
    {
        private readonly IRmaDbOperations _rmaDbOperations;

        public ResponsiblePersonsController(IRmaDbOperations rmaDbOperations, ILogger logger, IHubContext<NotificationHub> hubContext)
        : base(logger, hubContext)
        {
            _rmaDbOperations = rmaDbOperations;
        }

        [HttpGet]
        public IActionResult GetResponsiblePersons()
        {
            return ExecuteAction(() => _rmaDbOperations.GetResponsiblePersons());
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddResponsiblePerson(ResponsiblePersonDto responsiblePerson)
        {
            return ExecuteRepositoryActionAndNotify(() => _rmaDbOperations.AddResponsiblePerson(responsiblePerson), RepositoryType.ResponsiblePerson, RepositoryAction.Add);
        }

        [HttpPost]
        [Route("update")]
        public IActionResult UpdateResponsiblePerson(ResponsiblePersonDto responsiblePerson)
        {
            return ExecuteRepositoryActionAndNotify(() => _rmaDbOperations.UpdateResponsiblePerson(responsiblePerson), RepositoryType.ResponsiblePerson, RepositoryAction.Update);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteResponsiblePerson(int id)
        {
            return ExecuteRepositoryActionAndNotify(() => _rmaDbOperations.DeleteResponsiblePerson(id), RepositoryType.ResponsiblePerson, RepositoryAction.Delete);
        }
    }
}