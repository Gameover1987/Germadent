using Germadent.UserManagementCenter.Model;
using Germadent.WebApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Germadent.WebApi.Controllers.UserManagement
{
    [Route("api/userManagement/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IUmcDbOperations _umcDbOperations;

        public RolesController(IUmcDbOperations umcDbOperations)
        {
            _umcDbOperations = umcDbOperations;
        }

        [HttpPost]
        public RoleDto AddRole(RoleDto roleDto)
        {
            return _umcDbOperations.AddRole(roleDto);
        }
    }
}