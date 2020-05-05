using System;
using System.Linq;
using Germadent.UserManagementCenter.Model;
using Germadent.UserManagementCenter.Model.Rights;
using Germadent.WebApi.Configuration;

namespace Germadent.WebApi.Repository
{
    public class UmcDbOperations : IUmcDbOperations
    {
        private readonly UmcDbContext _dbContext;

        public UmcDbOperations(IServiceConfiguration configuration)
        {
            _dbContext = new UmcDbContext(configuration);
        }

        public UserDto[] GetUsers()
        {
            return _dbContext.Users.Select(x => new UserDto
            {
                Description = x.Description, 
                FullName = x.FullName, 
                Login = x.Login, 
                Password = x.Password,
                UserId = x.UserId
            }).ToArray();
        }

        public UserDto AddUser(UserDto userDto)
        {
            throw new NotImplementedException();
        }

        public RoleDto[] GetRoles()
        {
            throw new NotImplementedException();
        }

        public RoleDto AddRole(RoleDto roleDto)
        {
            throw new NotImplementedException();
        }

        public RightDto[] GetRights()
        {
            throw new NotImplementedException();
        }
    }
}
