using System.Linq;
using Germadent.UserManagementCenter.Model;
using Germadent.UserManagementCenter.Model.Rights;

namespace Germadent.WebApi.Entities.Conversion
{
    public interface IUmcEntityConverter
    {
        UserDto ConvertToUserDto(UserEntity entity);

        RoleDto ConvertToRoleDto(RoleEntity entity);

        RightDto ConvertToRightDto(RightEntity entity);

        RightInRoleEntity ConvertToRightInRoleEntity(int roleId, RightDto dto);

        RoleEntity ConvertToRoleEntity(RoleDto dto);

        RightEntity ConvertToRightEntity(RightDto dto);

        UserEntity ConvertToUserEntity(UserDto userDto);

        RoleInUserEntity ConvertToRoleInUserEntity(in int userId, RoleDto roleDto);
    }

    public class UmcEntityConverter : IUmcEntityConverter
    {
        public UserDto ConvertToUserDto(UserEntity entity)
        {
            return new UserDto
            {
                UserId = entity.UserId,
                Description = entity.Description,
                FullName = entity.FullName,
                Login = entity.Login,
                Password = entity.Password,
                Roles = entity.RolesInUser.Select(x => ConvertToRoleDto(x.RoleEntity)).ToArray()
            };
        }

        public RoleDto ConvertToRoleDto(RoleEntity entity)
        {
            return new RoleDto
            {
                RoleId = entity.RoleId,
                Name = entity.Name,
                Rights = entity.RightsInRole?.Select(x => ConvertToRightDto(x.RightEntity)).ToArray()
            };
        }

        public RightDto ConvertToRightDto(RightEntity entity)
        {
            return new RightDto
            {
                RightId = entity.RightId,
                ApplicationName = entity.ApplicationName,
                RightName = entity.RightName
            };
        }

        public RoleEntity ConvertToRoleEntity(RoleDto dto)
        {
            return new RoleEntity
            {
                RoleId = dto.RoleId,
                Name = dto.Name,
            };
        }

        public RightInRoleEntity ConvertToRightInRoleEntity(int roleId, RightDto dto)
        {
            return new RightInRoleEntity
            {
                RoleEntityId = roleId,
                RightEntityId = dto.RightId
            };
        }

        public RightEntity ConvertToRightEntity(RightDto dto)
        {
            return new RightEntity
            {
                RightId = dto.RightId,
                ApplicationName = dto.ApplicationName,
                RightName = dto.RightName
            };
        }

        public UserEntity ConvertToUserEntity(UserDto userDto)
        {
            return new UserEntity
            {
                FullName = userDto.FullName,
                Description = userDto.Description,
                Login = userDto.Login,
                Password = userDto.Password
            };
        }

        public RoleInUserEntity ConvertToRoleInUserEntity(in int userId, RoleDto roleDto)
        {
            return new RoleInUserEntity
            {
                UserEntityId = userId,
                RoleEntityId = roleDto.RoleId
            };
        }
    }
}