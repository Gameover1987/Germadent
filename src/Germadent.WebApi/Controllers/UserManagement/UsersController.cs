using System;
using Germadent.Common.FileSystem;
using Germadent.Common.Logging;
using Germadent.Model;
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
        private readonly IFileManager _fileManager;

        public UsersController(IUmcDbOperations umcDbOperations, ILogger logger, IFileManager fileManager)
        {
            _umcDbOperations = umcDbOperations;
            _logger = logger;
            _fileManager = fileManager;
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
        public IActionResult AddUser([FromBody]UserDto user)
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

        [HttpPost]
        [Route("fileUpload/{userId}/{fileName}")]
        public IActionResult FileUpload(int userId, string fileName)
        {
            try
            {
                _logger.Info(nameof(FileUpload));
                var stream = Request.Form.Files.GetFile("DataFile").OpenReadStream();
                _umcDbOperations.SetUserImage(userId, fileName, stream);
                return Ok();
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        [HttpGet]
        [Route("fileDownload/{userId}")]
        public IActionResult FileDownload(int userId)
        {
            try
            {
                _logger.Info(nameof(FileDownload));
                var fullFileName = _umcDbOperations.GetUserImage(userId);
                if (fullFileName == null)
                    return null;

                var stream = _fileManager.OpenFileAsStream(fullFileName);

                var fileStreamResult = new FileStreamResult(stream, "application/octet-stream");
                return fileStreamResult;
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