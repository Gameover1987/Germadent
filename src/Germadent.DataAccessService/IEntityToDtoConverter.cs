using Germadent.DataAccessService.Entities;
using Germadent.Rma.Model;

namespace Germadent.DataAccessService
{
    public interface IEntityToDtoConverter
    {
        OrderLite ConvertToOrderLite(OrderLiteEntity entity);

        Material ConvertToMaterial(MaterialEntity entity);
    }

    public class EntityToDtoConverter : IEntityToDtoConverter
    {
        public OrderLite ConvertToOrderLite(OrderLiteEntity entity)
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

        public Material ConvertToMaterial(MaterialEntity entity)
        {
            return new Material
            {
                Name = entity.MaterialName,
                IsObsolete = entity.FlagUnused
            };
        }
    }
}
