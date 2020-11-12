﻿using Germadent.Common.Extensions;
using Germadent.Common.FileSystem;
using Germadent.Common.Web;
using Germadent.UserManagementCenter.App.Configuration;
using Germadent.UserManagementCenter.Model;
using Germadent.UserManagementCenter.Model.Rights;
using RestSharp;

namespace Germadent.UserManagementCenter.App.ServiceClient
{
    public class UmcServiceClient : ServiceClientBase, IUmcServiceClient
    {
        private readonly IUmcConfiguration _configuration;
        private readonly IFileManager _fileManager;
        private readonly RestClient _client;

        public UmcServiceClient(IUmcConfiguration configuration, IFileManager fileManager)
        {
            _configuration = configuration;
            _fileManager = fileManager;
            _client = new RestClient();
        }

        public UserDto[] GetUsers()
        {
            return ExecuteHttpGet<UserDto[]>(_configuration.DataServiceUrl + "/api/userManagement/users/GetUsers");
        }

        public UserDto GetUserById(int id)
        {
            return ExecuteHttpGet<UserDto>(_configuration.DataServiceUrl + $"/api/userManagement/users/{id}");
        }

        public UserDto AddUser(UserDto userDto)
        {
            var addedUser = ExecuteHttpPost<UserDto>(_configuration.DataServiceUrl + "/api/userManagement/users/adduser", userDto);
            if (userDto.FileName != null)
            {
                var api = string.Format("{0}/api/userManagement/users/fileupload/{1}/{2}", _configuration.DataServiceUrl, addedUser.UserId, _fileManager.GetShortFileName(userDto.FileName));
                ExecuteFileUpload(api, userDto.FileName);
            }

            return addedUser;
        }

        public UserDto EditUser(UserDto userDto)
        {
            var editedUser = ExecuteHttpPost<UserDto>(_configuration.DataServiceUrl + "/api/userManagement/users/edituser", userDto);
            if (userDto.FileName != null)
            {
                var api = string.Format("{0}/api/userManagement/users/fileupload/{1}/{2}", _configuration.DataServiceUrl, editedUser.UserId, _fileManager.GetShortFileName(editedUser.FileName));
                ExecuteFileUpload(api, userDto.FileName);
            }

            return editedUser;
        }

        public void DeleteUser(int userId)
        {
            ExecuteHttpDelete(_configuration.DataServiceUrl + "/api/userManagement/users/deleteuser/" + userId);
        }

        public RoleDto[] GetRoles()
        {
            return ExecuteHttpGet<RoleDto[]>(_configuration.DataServiceUrl + "/api/userManagement/roles");
        }

        public RoleDto AddRole(RoleDto role)
        {
            return ExecuteHttpPost<RoleDto>(_configuration.DataServiceUrl + "/api/userManagement/roles/addrole", role);
        }

        public RoleDto EditRole(RoleDto role)
        {
            return ExecuteHttpPost<RoleDto>(_configuration.DataServiceUrl + "/api/userManagement/roles/editrole", role);
        }

        public void DeleteRole(int roleId)
        {
            ExecuteHttpDelete(_configuration.DataServiceUrl + "/api/userManagement/roles/deleterole/" + roleId);
        }

        public RightDto[] GetRights()
        {
            return ExecuteHttpGet<RightDto[]>(_configuration.DataServiceUrl + "/api/userManagement/rights");
        }

        public RightDto[] GetRightsByRole(int roleId)
        {
            throw new System.NotImplementedException();
        }

        protected override void HandleError(IRestResponse response)
        {
            throw new ServerSideException(response);
        }
    }
}