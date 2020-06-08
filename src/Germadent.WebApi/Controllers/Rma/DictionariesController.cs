using System;
using Germadent.Common.Logging;
using Germadent.Rma.Model;
using Germadent.WebApi.DataAccess;
using Germadent.WebApi.DataAccess.Rma;
using Microsoft.AspNetCore.Mvc;

namespace Germadent.WebApi.Controllers.Rma
{
    [Route("api/Rma/[controller]")]
    [ApiController]
    public class DictionariesController : ControllerBase
    {
        private readonly IRmaDbOperations _rmaDbOperations;
        private readonly ILogger _logger;

        public DictionariesController(IRmaDbOperations rmaDbOperations, ILogger logger)
        {
            _rmaDbOperations = rmaDbOperations;
            _logger = logger;
        }

        [HttpGet("{dictionaryTypeStr}")]
        public IActionResult GetDictionary(string dictionaryTypeStr)
        {
            try
            {
                _logger.Info(nameof(GetDictionary));
                if (!Enum.TryParse(typeof(DictionaryType), dictionaryTypeStr, out var dictionaryType))
                {
                    throw new ArgumentException("Неизвестный тип словаря");
                }

                return Ok(_rmaDbOperations.GetDictionary((DictionaryType) dictionaryType));
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }
    }
}