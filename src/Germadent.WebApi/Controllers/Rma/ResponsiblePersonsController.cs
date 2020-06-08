using System;
using Germadent.Common.Logging;
using Germadent.Rma.Model;
using Germadent.WebApi.DataAccess;
using Germadent.WebApi.DataAccess.Rma;
using Microsoft.AspNetCore.Mvc;

namespace Germadent.WebApi.Controllers.Rma
{
    [Route("api/Rma/ResponsiblePersons")]
    [ApiController]
    public class ResponsiblePersonsController : ControllerBase
    {
        private readonly IRmaDbOperations _rmaDbOperations;
        private readonly ILogger _logger;

        public ResponsiblePersonsController(IRmaDbOperations rmaDbOperations, ILogger logger)
        {
            _rmaDbOperations = rmaDbOperations;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetResponsiblePersons()
        {
            try
            {
                _logger.Info(nameof(GetResponsiblePersons));
                return Ok(_rmaDbOperations.GetResponsiblePersons());
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        [HttpPost]
        public IActionResult AddResponsiblePerson(ResponsiblePersonDto responsiblePerson)
        {
            try
            {
                _logger.Info(nameof(AddResponsiblePerson));
                return Ok(_rmaDbOperations.AddResponsiblePerson(responsiblePerson));
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        [HttpPut]
        public IActionResult UpdateResponsiblePerson(ResponsiblePersonDto responsiblePerson)
        {
            try
            {
                _logger.Info(nameof(UpdateResponsiblePerson));
                return Ok(_rmaDbOperations.UpdateResponsiblePerson(responsiblePerson));
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteResponsiblePerson(int id)
        {
            try
            {
                _logger.Info(nameof(DeleteResponsiblePerson));
                return Ok(_rmaDbOperations.DeleteResponsiblePerson(id));
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }
    }
}