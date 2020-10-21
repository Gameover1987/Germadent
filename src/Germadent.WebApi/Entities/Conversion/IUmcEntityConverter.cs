using System.Linq;
using Germadent.UserManagementCenter.Model;
using Germadent.UserManagementCenter.Model.Rights;

namespace Germadent.WebApi.Entities.Conversion
{
    public interface IUmcEntityConverter
    {
        RightDto ConvertToRightDto(RightEntity entity);
    }

    public class UmcEntityConverter : IUmcEntityConverter
    {
        public RightDto ConvertToRightDto(RightEntity entity)
        {
            return new RightDto
            {
                RightId = entity.RightId,
                RightName = entity.RightName,
                RightDescription = entity.RightDescription,
                ApplicationModule = (ApplicationModule)entity.ApplicationId,
                IsObsolete = entity.IsObsolete
            };
        }
    }
}