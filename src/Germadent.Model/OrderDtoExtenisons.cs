﻿namespace Germadent.Model
{
    public static class OrderDtoExtenisons
    {
        public static OrderLiteDto ToOrderLite(this OrderDto orderDto)
        {
            return new OrderLiteDto
            {
                WorkOrderId = orderDto.WorkOrderId,
                BranchType = orderDto.BranchType,
                Created = orderDto.Created,
                CustomerName = orderDto.Customer,
                PatientFnp = orderDto.Patient,
                DoctorFullName = orderDto.ResponsiblePerson,
                Status = orderDto.Status,
                StatusChanged = orderDto.StatusChanged,
                DocNumber = orderDto.DocNumber,
                CreatorFullName = orderDto.CreatorFullName
            };
        }
    }
}
