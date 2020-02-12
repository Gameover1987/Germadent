using Germadent.Rma.Model;

namespace Germadent.DataAccessService.Entities.Conversion
{
    public interface IEntityToDtoConverter
    {
        OrderDto ConvertToOrder(OrderEntity entity);

        ToothDto ConvertToTooth(ToothEntity entity);

        OrderLiteDto ConvertToOrderLite(OrderLiteEntity entity);

        ProstheticConditionDto ConvertToProstheticCondition(ProstheticConditionEntity entity);

        MaterialDto ConvertToMaterial(MaterialEntity entity);

        ProstheticsTypeDto ConvertToProstheticType(ProstheticTypeEntity entity);
        TransparencesDto ConvertToTransparences(TransparencesEntity entity);
    }
}
