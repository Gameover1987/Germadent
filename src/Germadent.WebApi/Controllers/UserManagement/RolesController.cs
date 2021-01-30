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
        public IActionResult GetRoles()
        {
            try
            {
                _logger.Info(nameof(GetRoles));
                var roles = _umcDbOperations.GetRoles();
                return Ok(roles);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }
        
        [HttpPost]
        public IActionResult AddRole(RoleDto roleDto)
        {
            try
            {
                _logger.Info(nameof(EditRole));
                return Ok(_umcDbOperations.AddRole(roleDto));
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        [HttpPost]
        public IActionResult EditRole(RoleDto roleDto)
        {
            try
            {
                _logger.Info(nameof(EditRole));
                return Ok(_umcDbOperations.UpdateRole(roleDto));
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
           
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