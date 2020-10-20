using System;
using Germadent.Common.Logging;
using Germadent.UserManagementCenter.Model;
using Germadent.WebApi.DataAccess;
using Germadent.WebApi.DataAccess.UserManagement;
using Microsoft.AspNetCore.Mvc;

namespace Germadent.WebApi.Controllers.UserManagement
{
    [Route("api/userManagement/roles/{action=GetRoles}")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IUmcDbOperations _umcDbOperations;
        private readonly ILogger _logger;

        public RolesController(IUmcDbOperations umcDbOperations, ILogger logger)
        {
            _umcDbOperations = umcDbOperations;
            _logger = logger;
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

        [HttpDelete("{roleId:int}")]

        public IActionResult DeleteRole(int roleId)
        {
            try
            {
                _logger.Info(nameof(DeleteRole));
                _umcDbOperations.DeleteRole(roleId);
                return Ok();
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }
    }
}