using Germadent.Common.Extensions;
using Germadent.UserManagementCenter.App.Configuration;
using Germadent.UserManagementCenter.Model;
using Germadent.UserManagementCenter.Model.Rights;
using RestSharp;

namespace Germadent.UserManagementCenter.App.ServiceClient
{
    public class UmcServiceClient : IUmcServiceClient
    {
        private readonly IUmcConfiguration _configuration;
        private readonly RestClient _client;

        public UmcServiceClient(IUmcConfiguration configuration)
        {
            _configuration = configuration;

            _client = new RestClient();
        }

        public UserDto[] GetUsers()
        {
            return ExecuteHttpGet<UserDto[]>("/api/userManagement/users");
        }

        public RoleDto[] GetRoles()
        {
            return ExecuteHttpGet<RoleDto[]>("/api/userManagement/roles");
        }

        public RoleDto AddRole(RoleDto role)
        {
            return ExecuteHttpPost<RoleDto>("/api/userManagement/roles/addrole", role);
        }

        public RoleDto EditRole(RoleDto role)
        {
            return ExecuteHttpPost<RoleDto>("/api/userManagement/roles/editrole", role);
        }

        public RightDto[] GetRights()
        {
            return ExecuteHttpGet<RightDto[]>("/api/userManagement/rights");
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