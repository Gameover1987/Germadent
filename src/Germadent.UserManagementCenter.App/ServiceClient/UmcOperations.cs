using Germadent.Common.Extensions;
using Germadent.UserManagementCenter.App.Configuration;
using Germadent.UserManagementCenter.Model;
using Germadent.UserManagementCenter.Model.Rights;
using RestSharp;

namespace Germadent.UserManagementCenter.App.ServiceClient
{
    public class UmcOperations : IUmcOperations
    {
        private readonly IUmcConfiguration _configuration;
        private readonly RestClient _client;

        public UmcOperations(IUmcConfiguration configuration)
        {
            _configuration = configuration;

            _client = new RestClient();
        }

        public UserDto[] GetUsers()
        {
            return new UserDto[0];
            //return ExecuteHttpGet<UserDto[]>("/api/userManagement/users");
        }

        public RoleDto[] GetRoles()
        {
            return new RoleDto[0];
        }

        public RoleDto AddRole(RoleDto role)
        {
            return ExecuteHttpPost<RoleDto>("/api/userManagement/roles", role);
        }

        public RightDto[] GetRights()
        {
            throw new System.NotImplementedException();
        }

        public RightDto[] GetRightsByRole(int roleId)
        {
            throw new System.NotImplementedException();
        }

        private T ExecuteHttpGet<T>(string api)
        {
            var restRequest = new RestRequest(_configuration.DataServiceUrl + api, Method.GET);
            restRequest.RequestFormat = DataFormat.Json;
            var response = _client.Execute(restRequest, Method.GET);
            return response.Content.DeserializeFromJson<T>();
        }

        private T ExecuteHttpPost<T>(string api, object body, byte[] file = null)
        {
            var restRequest = new RestRequest(_configuration.DataServiceUrl + api, Method.POST);

            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddHeader("Accept", "application/json");

            restRequest.AddJsonBody(body);

            if (file != null)
            {
                restRequest.AddHeader("Content-Type", "multipart/form-data");
                restRequest.AddFile("DataFile", file, "DataFile");
            }

            var response = _client.Execute(restRequest, Method.POST);
            return response.Content.DeserializeFromJson<T>();
        }
    }
}