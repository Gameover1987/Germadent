using System.Collections.Generic;
using Germadent.UserManagementCenter.Model;
using Germadent.UserManagementCenter.Model.Rights;
using Germadent.WebApi.Configuration;
using Germadent.WebApi.Entities;
using Germadent.WebApi.Entities.Conversion;

namespace Germadent.WebApi.DataAccess.UserManagement
{
    public class UmcDbOperations : IUmcDbOperations
    {
        private readonly IUmcEntityConverter _converter;

        public UmcDbOperations(IServiceConfiguration configuration, IUmcEntityConverter converter)
        {
            _converter = converter;

            ActualizeRights();
        }

        public UserDto[] GetUsers()
        {
           return new UserDto[0];
        }

        public UserDto GetUserById(int id)
        {
            return null;
        }

        public UserDto AddUser(UserDto userDto)
        {
            return null;
        }

        public void UpdateUser(UserDto userDto)
        {
            
        }

        public RoleDto[] GetRoles()
        {
            return null;
        }

        public RoleDto AddRole(RoleDto roleDto)
        {
            return null;
        }

        public void UpdateRole(RoleDto roleDto)
        {
            
        }

        public RightDto[] GetRights()
        {
            return null;
        }

        private void ActualizeRights()
        {
            //var oldRights = _dbContext.Rights.ToArray();

            //var newRights = UserRightsProvider.GetAllUserRights().Select(x => new RightEntity
            //{
            //    RightId = x.RightId,
            //    ApplicationName = x.ApplicationName,
            //    RightName = x.RightName
            //}).ToArray();

            //var rightComparer = new RightComparer();

            //var rightsToDelete = oldRights.Except(newRights, rightComparer).ToArray();
            //_dbContext.Rights.RemoveRange(rightsToDelete);

            //var rightsToAdd = newRights.Except(oldRights, rightComparer).ToArray();
            //_dbContext.Rights.AddRange(rightsToAdd);

            //_dbContext.SaveChanges();
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
