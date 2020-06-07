using System;
using Germadent.Common.Logging;
using Germadent.Rma.Model;
using Germadent.WebApi.DataAccess;
using Germadent.WebApi.DataAccess.Rma;
using Microsoft.AspNetCore.Mvc;

namespace Germadent.WebApi.Controllers.Rma
{
    [Route("api/Rma/Customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IRmaDbOperations _rmaDbOperations;
        private readonly ILogger _logger;

        public CustomersController(IRmaDbOperations rmaDbOperations, ILogger logger)
        {
            _rmaDbOperations = rmaDbOperations;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetCustomers(string mask)
        {
            try
            {
                var customers = _rmaDbOperations.GetCustomers(mask);
                return Ok(customers);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        [HttpPost]
        public IActionResult AddCustomer(CustomerDto customer)
        {
            try
            {
                return Ok(_rmaDbOperations.AddCustomer(customer));
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        [HttpPut]
        public IActionResult UpdateCustomer(CustomerDto customer)
        {
            try
            {
                return Ok(_rmaDbOperations.UpdateCustomer(customer));
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteCustomer(int id)
        {
            try
            {
                return Ok(_rmaDbOperations.DeleteCustomer(id));
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }
    }
}