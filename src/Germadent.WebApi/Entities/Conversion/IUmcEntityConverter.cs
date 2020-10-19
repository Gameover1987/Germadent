using System.Linq;
using Germadent.UserManagementCenter.Model;
using Germadent.UserManagementCenter.Model.Rights;

namespace Germadent.WebApi.Entities.Conversion
{
    public interface IUmcEntityConverter
    {

       

        RightDto ConvertToRightDto(RightEntity entity);

     

        RightEntity ConvertToRightEntity(RightDto dto);
    }

    public class UmcEntityConverter : IUmcEntityConverter
    {
        


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

       
    }
}