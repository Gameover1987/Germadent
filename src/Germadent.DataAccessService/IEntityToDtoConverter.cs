using Germadent.DataAccessService.Entitties;
using Germadent.Rma.Model;

namespace Germadent.DataAccessService
{
    public interface IEntityToDtoConverter
    {
        OrderLite ConvertFrom(OrderLiteEntity entity);
    }

    public class EntityToDtoConverter : IEntityToDtoConverter
    {
        public OrderLite ConvertFrom(OrderLiteEntity entity)
        {
            return new OrderLite
            {
                BranchType = (BranchType)entity.BranchTypeId,
                BranchTypeId = entity.BranchTypeId,
                CustomerName = entity.CustomerName,
                PatientFnp = entity.PatientFnp,
                DocNumber = entity.DocNumber,
                ResponsiblePerson = entity.ResponsiblePersonName,
                Status = entity.Status,
                Created = entity.Created,
                Closed = entity.Closed
            };
        }
    }
}
