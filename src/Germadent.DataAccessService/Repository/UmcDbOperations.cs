using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.DataAccessService.Configuration;
using Germadent.UserManagementCenter.Model;
using Germadent.UserManagementCenter.Model.Rights;

namespace Germadent.DataAccessService.Repository
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
