using Germadent.UserManagementCenter.Model;
using Germadent.WebApi.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Germadent.WebApi.Controllers.UserManagement
{
    [Route("api/userManagement/users/{action=GetUsers}")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUmcDbOperations _umcDbOperations;

        public UsersController(IUmcDbOperations umcDbOperations)
        {
            _umcDbOperations = umcDbOperations;
        }

        [HttpGet]
        public UserDto[] GetUsers()
        {
            return _umcDbOperations.GetUsers();
        }

        [HttpPost]
        public UserDto AddUser(UserDto user)
        {
            return _umcDbOperations.AddUser(user);
        }

        [HttpPost]
        public UserDto EditUser(UserDto userDto)
        {
            _umcDbOperations.UpdateUser(userDto);
            return userDto;
        }
    }
}