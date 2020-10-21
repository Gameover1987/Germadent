using System;
using Germadent.Common.Logging;
using Germadent.UserManagementCenter.Model;
using Germadent.WebApi.DataAccess.UserManagement;
using Microsoft.AspNetCore.Mvc;

namespace Germadent.WebApi.Controllers.UserManagement
{
    [Route("api/userManagement/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUmcDbOperations _umcDbOperations;
        private readonly ILogger _logger;

        public UsersController(IUmcDbOperations umcDbOperations, ILogger logger)
        {
            _umcDbOperations = umcDbOperations;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetUsers")]
        public IActionResult GetUsers()
        {
            try
            {
                _logger.Info(nameof(GetUsers));
                var users = _umcDbOperations.GetUsers();
                return Ok(users);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        [HttpGet("{id:int}")]
        [Route("GetById")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                _logger.Info(nameof(GetUserById));
                return Ok(_umcDbOperations.GetUserById(id));
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        [HttpPost]
        [Route("AddUser")]
        public IActionResult AddUser(UserDto user)
        {
            try
            {
                _logger.Info(nameof(AddUser));
                return Ok(_umcDbOperations.AddUser(user));
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        [HttpPost]
        [Route("EditUser")]
        public IActionResult EditUser(UserDto userDto)
        {
            try
            {
                _logger.Info(nameof(EditUser));
                return Ok(_umcDbOperations.UpdateUser(userDto));
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        [HttpDelete]
        [Route("DeleteUser/{userId}")]

        public IActionResult DeleteUser(int userId)
        {
            try
            {
                _logger.Info(nameof(DeleteUser));
                _umcDbOperations.DeleteUser(userId);
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