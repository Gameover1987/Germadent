using Germadent.UserManagementCenter.Model;
using Germadent.WebApi.DataAccess;
using Germadent.WebApi.DataAccess.UserManagement;
using Microsoft.AspNetCore.Mvc;

namespace Germadent.WebApi.Controllers.UserManagement
{
    [Route("api/userManagement/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUmcDbOperations _umcDbOperations;

        public UsersController(IUmcDbOperations umcDbOperations)
        {
            _umcDbOperations = umcDbOperations;
        }

        [HttpGet]
        [Route("GetUsers")]
        public UserDto[] GetUsers()
        {
            return _umcDbOperations.GetUsers();
        }

        [HttpGet("{id:int}")]
        [Route("GetById")]
        public UserDto GetUserById(int id)
        {
            return _umcDbOperations.GetUserById(id);
        }

        [HttpPost]
        [Route("AddUser")]
        public UserDto AddUser(UserDto user)
        {
            return _umcDbOperations.AddUser(user);
        }

        [HttpPost]
        [Route("EditUser")]
        public UserDto EditUser(UserDto userDto)
        {
            _umcDbOperations.UpdateUser(userDto);
            return userDto;
        }

        [HttpDelete]
        [Route("DeleteUser/{userId}")]

        public IActionResult DeleteUser(int userId)
        {
            _umcDbOperations.DeleteUser(userId);
            return Ok();
        }
    }
}