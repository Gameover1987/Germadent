using Germadent.UserManagementCenter.Model;
using Germadent.WebApi.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Germadent.WebApi.Controllers.UserManagement
{
    [Route("api/userManagement/roles/{action=GetRoles}")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IUmcDbOperations _umcDbOperations;

        public RolesController(IUmcDbOperations umcDbOperations)
        {
            _umcDbOperations = umcDbOperations;
        }


        [HttpGet]
        public RoleDto[] GetRoles()
        {
            return _umcDbOperations.GetRoles();
        }
        
        [HttpPost]
        public RoleDto AddRole(RoleDto roleDto)
        {
            return _umcDbOperations.AddRole(roleDto);
        }

        [HttpPost]
        public RoleDto EditRole(RoleDto roleDto)
        {
            _umcDbOperations.UpdateRole(roleDto);
            return roleDto;
        }
    }
}