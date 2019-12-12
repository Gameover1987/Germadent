﻿using Germadent.Rma.Model;

namespace Germadent.DataAccessService.Entities.Conversion
{
    public interface IEntityToDtoConverter
    {
        OrderDto ConvertToOrder(OrderEntity entity);

        OrderLiteDto ConvertToOrderLite(OrderLiteEntity entity);

        MaterialDto ConvertToMaterial(MaterialEntity entity);
    }
}
