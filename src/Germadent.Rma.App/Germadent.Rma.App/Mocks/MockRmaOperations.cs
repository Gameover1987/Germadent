using System;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Mocks
{
    public class MockRmaOperations : IRmaOperations
    {
        public OrderLiteDto[] GetOrders(OrdersFilter ordersFilter = null)
        {
            return new OrderLiteDto[]
            {
                new OrderLiteDto
                {
                    BranchType = BranchType.Laboratory,
                    Closed = DateTime.Now,
                    Created = DateTime.Now.AddDays(-1),
                    CustomerName = "ООО Рога и копыта",
                    DocNumber = "001-ЗТЛ",
                    DoctorFullName = "Докторов Доктор Докторович",
                    PatientFnp = "Пациентов Пациент Пациентович",
                    TechnicFullName = "Техник Техникович Техников",
                    WorkOrderId = 1
                },
                new OrderLiteDto
                {
                    BranchType = BranchType.MillingCenter,
                    Closed = DateTime.Now,
                    Created = DateTime.Now.AddDays(-1),
                    CustomerName = "ООО Рога и копыта",
                    DocNumber = "001-ФЦ",
                    DoctorFullName = "Докторов Доктор Докторович",
                    PatientFnp = "Пациентов Пациент Пациентович",
                    TechnicFullName = "Техник Техникович Техников",
                    WorkOrderId = 2
                },
            };
        }

        public OrderDto GetOrderDetails(int id)
        {
            throw new NotImplementedException();
        }

        public MaterialDto[] GetMaterials()
        {
            var materials = new[]
            {
                new MaterialDto {Name = "ZrO"},
                new MaterialDto {Name = "PMMA mono"},
                new MaterialDto {Name = "PMMA multi"},
                new MaterialDto {Name = "WAX"},
                new MaterialDto {Name = "MIK"},
                new MaterialDto {Name = "CAD-Temp mono"},
                new MaterialDto {Name = "CAD-Temp multi"},
                new MaterialDto {Name = "Enamik mono"},
                new MaterialDto {Name = "Enamik multi"},
                new MaterialDto {Name = "SUPRINITY"},
                new MaterialDto {Name = "Mark II"},
                new MaterialDto {Name = "WAX"},
                new MaterialDto {Name = "TriLuxe forte"},
                new MaterialDto {Name = "Ti"},
                new MaterialDto {Name = "E.MAX"},
            };

            return materials;
        }

        public ProstheticsTypeDto[] GetProstheticTypes()
        {
            throw new NotImplementedException();
        }

        public OrderDto AddOrder(OrderDto order)
        {
            throw new NotImplementedException();
        }

        public ToothDto[] AddOrUpdateToothCard(ToothDto[] toothCard)
        {
            throw new NotImplementedException();
        }

        public OrderDto UpdateOrder(OrderDto order)
        {
            throw new NotImplementedException();
        }
    }
}