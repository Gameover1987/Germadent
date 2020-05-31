using System;
using Germadent.Rma.Model;
using Germadent.WebApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Germadent.WebApi.Controllers.Rma
{
    [Route("api/Rma/[controller]")]
    [ApiController]
    public class DictionariesController : ControllerBase
    {
        private readonly IRmaDbOperations _rmaDbOperations;

        public DictionariesController(IRmaDbOperations rmaDbOperations)
        {
            _rmaDbOperations = rmaDbOperations;
        }

        [HttpGet("{dictionaryTypeStr}")]
        public DictionaryItemDto[] GetDictionary(string dictionaryTypeStr)
        {
            object dictionaryType;

            if (Enum.TryParse(typeof(DictionaryType), dictionaryTypeStr, out dictionaryType))
            {
                return _rmaDbOperations.GetDictionary((DictionaryType)dictionaryType);
            }

            throw new ArgumentException("");
        }
    }
}