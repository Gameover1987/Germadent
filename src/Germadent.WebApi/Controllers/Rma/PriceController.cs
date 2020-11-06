using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Germadent.Common.Logging;
using Germadent.Rma.Model;
using Germadent.Rma.Model.Pricing;
using Germadent.WebApi.DataAccess.Rma;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Germadent.WebApi.Controllers.Rma
{
    [Route("api/Rma/Pricing")]
    [ApiController]
    public class PriceController : ControllerBase
    {
        private readonly IRmaDbOperations _rmaDbOperations;
        private readonly ILogger _logger;

        public PriceController(IRmaDbOperations rmaDbOperations, ILogger logger)
        {
            _rmaDbOperations = rmaDbOperations;
            _logger = logger;
        }

        [HttpGet]
        [Route("PriceGroups/{branchType:int}")]
        public IActionResult GetPriceGroups(int branchType)
        {
            try
            {
                _logger.Info(nameof(GetPriceGroups));
                var priceGroups = _rmaDbOperations.GetPriceGroups((BranchType)branchType);
                return Ok(priceGroups);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        [HttpPost]
        [Route("AddPriceGroup")]
        public IActionResult AddPriceGroup(PriceGroupDto priceGroup)
        {
            try
            {
                _logger.Info(nameof(AddPriceGroup));
                priceGroup = _rmaDbOperations.AddPriceGroup(priceGroup);
                return Ok(priceGroup);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        [HttpGet]
        [Route("PricePositions/{branchType:int}")]
        public IActionResult GetPricePositions(int branchType)
        {
            try
            {
                _logger.Info(nameof(GetPricePositions));
                var priceGroups = _rmaDbOperations.GetPricePositions((BranchType)branchType);
                return Ok(priceGroups);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }


        [HttpGet]
        [Route("Products")]
        public IActionResult GetProducts()
        {
            try
            {
                _logger.Info(nameof(GetProducts));
                var products = _rmaDbOperations.GetProducts();
                return Ok(products);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }
    }
}
