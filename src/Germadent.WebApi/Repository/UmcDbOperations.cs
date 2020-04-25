using System;
using Germadent.UserManagementCenter.Model;
using Germadent.UserManagementCenter.Model.Rights;
using Germadent.WebApi.Configuration;

namespace Germadent.WebApi.Repository
{
    public class UmcDbOperations : IUmcDbOperations
    {
        private readonly IServiceConfiguration _configuration;



        public UmcDbOperations(IServiceConfiguration configuration)
        {
            _configuration = configuration;
        }

        public UserDto[] GetUsers()
        {
            throw new NotImplementedException();
        }

        public RoleDto[] GetRoles()
        {
            throw new NotImplementedException();
        }

        public RoleDto[] AddRole(RoleDto roleDto)
        {
            throw new NotImplementedException();
        }

        public RightDto[] GetRights()
        {
            throw new NotImplementedException();
        }
    }
}
