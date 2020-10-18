using System.Linq;
using Germadent.UserManagementCenter.Model;
using Germadent.UserManagementCenter.Model.Rights;

namespace Germadent.WebApi.Entities.Conversion
{
    public interface IUmcEntityConverter
    {
        UserDto ConvertToUserDto(UserEntity entity);

       

        RightDto ConvertToRightDto(RightEntity entity);

     

        RightEntity ConvertToRightEntity(RightDto dto);

        UserEntity ConvertToUserEntity(UserDto userDto);
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
    }
}