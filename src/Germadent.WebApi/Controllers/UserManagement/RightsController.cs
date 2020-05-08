using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Germadent.UserManagementCenter.Model;
using Germadent.UserManagementCenter.Model.Rights;
using Germadent.WebApi.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Germadent.WebApi.Controllers.UserManagement
{
    [Route("api/userManagement/[controller]")]
    [ApiController]
    public class RightsController : ControllerBase
    {
        private readonly IUmcDbOperations _umcDbOperations;

        public RightsController(IUmcDbOperations umcDbOperations)
        {
            _umcDbOperations = umcDbOperations;
        }


        [HttpGet]
        public RightDto[] GetRights()
        {
            return _umcDbOperations.GetRights();
        }
    }
}