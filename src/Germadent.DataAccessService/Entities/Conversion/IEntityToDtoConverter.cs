using Germadent.Rma.Model;

namespace Germadent.DataAccessService.Entities.Conversion
{
    public interface IEntityToDtoConverter
    {
        Order ConvertToOrder(OrderEntity entity);

        OrderLite ConvertToOrderLite(OrderLiteEntity entity);

        Material ConvertToMaterial(MaterialEntity entity);
    }
}
