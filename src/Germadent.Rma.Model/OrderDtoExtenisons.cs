using System;

namespace Germadent.Rma.Model
{
    public static class OrderDtoExtenisons
    {
        public static string GetOrderDocNumber(this OrderDto orderDto)
        {
            if (orderDto.BranchType == BranchType.Laboratory)
            {
                return $"{orderDto.WorkOrderId}-ЗТЛ";
            }
            else if (orderDto.BranchType == BranchType.MillingCenter)
            {
                return $"{orderDto.WorkOrderId}-ФЦ";
            }

            throw new NotSupportedException("Неизвестный тип филиала");
        }

        public static OrderLiteDto ToOrderLite(this  OrderDto orderDto)
        {
            return new OrderLiteDto
            {
                WorkOrderId = orderDto.WorkOrderId,
                BranchType = orderDto.BranchType,
                Closed = orderDto.Closed,
                Created = orderDto.Created,
                CustomerName = orderDto.Customer,
                PatientFnp = orderDto.Patient,
                ResponsiblePerson = orderDto.ResponsiblePerson,
                Status = orderDto.Status,
                DocNumber = orderDto.DocNumber
            };
        }
    }
}
