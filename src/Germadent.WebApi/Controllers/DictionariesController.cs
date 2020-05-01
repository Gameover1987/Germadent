using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Germadent.Rma.Model;
using Germadent.WebApi.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Germadent.WebApi.Controllers
{
    [Route("api/[controller]")]
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