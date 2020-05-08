using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Germadent.UserManagementCenter.Model;
using Germadent.UserManagementCenter.Model.Rights;
using Germadent.WebApi.Configuration;
using Germadent.WebApi.Entities;
using Germadent.WebApi.Entities.Conversion;

namespace Germadent.WebApi.Repository
{
    public class UmcDbOperations : IUmcDbOperations, IDisposable
    {
        private readonly IUmcEntityConverter _converter;
        private readonly UmcDbContext _dbContext;

        public UmcDbOperations(IServiceConfiguration configuration, IUmcEntityConverter converter)
        {
            _converter = converter;
            _dbContext = new UmcDbContext(configuration);

            ActualizeRights();
        }

        public UserDto[] GetUsers()
        {
            var users = _dbContext.Users.Select(x => _converter.ConvertToUserDto(x)).ToArray();
            return users;
        }

        public UserDto AddUser(UserDto userDto)
        {
            throw new NotImplementedException();
        }

        public RoleDto[] GetRoles()
        {
            var roles = _dbContext.Roles.Select(x => _converter.ConvertToRoleDto(x)).ToArray();
            return roles;
        }

        public RoleDto AddRole(RoleDto roleDto)
        {
            var role = _converter.ConvertToRoleEntity(roleDto);
            _dbContext.Roles.Add(role);
            _dbContext.SaveChanges();

            roleDto.RoleId = role.RoleId;

            return roleDto;
        }

        public RightDto[] GetRights()
        {
            var rights = _dbContext.Rights.Select(x => _converter.ConvertToRightDto(x)).ToArray();
            return rights;
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }

        private void ActualizeRights()
        {
            var oldRights = _dbContext.Rights.ToArray();

            var newRights = UserRightsProvider.GetAllUserRights().Select(x => new RightEntity
            {
                RightId = x.RightId,
                ApplicationName = x.ApplicationName,
                RightName = x.RightName
            }).ToArray();


            var rightComparer = new RightComparer();

            var rightsToDelete = oldRights.Except(newRights, rightComparer).ToArray();
            _dbContext.Rights.RemoveRange(rightsToDelete);

            var rightsToAdd = newRights.Except(oldRights, rightComparer).ToArray();
            _dbContext.Rights.AddRange(rightsToAdd);

            _dbContext.SaveChanges();
        }

        private class RightComparer : IEqualityComparer<RightEntity>
        {
            public bool Equals(RightEntity x, RightEntity y)
            {
                return x.RightName == y.RightName &&
                       x.ApplicationName == y.ApplicationName;
            }

            public int GetHashCode(RightEntity entity)
            {
                return entity.RightName.GetHashCode();
            }
        }
    }
}
