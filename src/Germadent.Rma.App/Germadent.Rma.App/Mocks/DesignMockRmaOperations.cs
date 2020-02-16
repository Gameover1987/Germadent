﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Mocks
{
    public class DesignMockRmaOperations : IRmaOperations
    {
        private readonly List<OrderDto> _orders = new List<OrderDto>();

        public DesignMockRmaOperations()
        {
            _orders.Add(new OrderDto
            {
                BranchType = BranchType.Laboratory,
                Closed = DateTime.Now,
                Created = DateTime.Now.AddDays(-1),
                Customer = "ООО Рога и копыта",
                DocNumber = "001-ЗТЛ",
                ResponsiblePerson = "Докторов Доктор Докторович",
                Patient = "Пациентов Пациент Пациентович",
                WorkOrderId = 1,
                ToothCard = new ToothDto[]
                {
                    new ToothDto
                    {
                        HasBridge = true,
                        MaterialId = 1,
                        MaterialName = "ZrO",
                        ProstheticsId = 1,
                        ProstheticsName = "Каркас",
                        ToothNumber = 11
                    },
                    new ToothDto
                    {
                        HasBridge = true,
                        MaterialId = 2,
                        MaterialName = "PMMA mono",
                        ProstheticsId = 3,
                        ProstheticsName = "Абатмент",
                        ToothNumber = 12
                    },
                }
            });
            _orders.Add(new OrderDto
            {
                BranchType = BranchType.MillingCenter,
                Closed = DateTime.Now,
                Created = DateTime.Now.AddDays(-1),
                Customer = "ООО Рога и копыта",
                DocNumber = "001-ФЦ",
                ResponsiblePerson = "Техников Техник Техникович",
                Patient = "Пациентов Пациент Пациентович",
                WorkOrderId = 2,
                ToothCard = new ToothDto[]
                {
                    new ToothDto
                    {
                        HasBridge = true,
                        MaterialId = 1,
                        MaterialName = "ZrO",
                        ProstheticsId = 1,
                        ProstheticsName = "Каркас",
                        ToothNumber = 11
                    },
                    new ToothDto
                    {
                        HasBridge = true,
                        MaterialId = 2,
                        MaterialName = "PMMA mono",
                        ProstheticsId = 3,
                        ProstheticsName = "Абатмент",
                        ToothNumber = 12
                    },
                }
            });
        }

        public OrderLiteDto[] GetOrders(OrdersFilter ordersFilter = null)
        {
            //Thread.Sleep(2000);
            return _orders.Select(x => x.ToOrderLite()).ToArray();
        }

        public OrderDto GetOrderDetails(int id)
        {
            //Thread.Sleep(1000);
            return _orders.First(x => x.WorkOrderId == id);
        }

        public ProstheticConditionDto[] GetProstheticConditions()
        {
            var ptostheticsConditions = new[]
            {
                new ProstheticConditionDto{Name = "Культя", Id = 1},
                new ProstheticConditionDto{Name = "Имплант", Id = 2},
            };

            return ptostheticsConditions;
        }

        public MaterialDto[] GetMaterials()
        {
            //Thread.Sleep(2000);
            var materials = new[]
            {
                new MaterialDto {MaterialName = "ZrO", Id = 1},
                new MaterialDto {MaterialName = "PMMA mono", Id = 2},
                new MaterialDto {MaterialName = "PMMA multi", Id = 3},
                new MaterialDto {MaterialName = "WAX", Id = 4},
                new MaterialDto {MaterialName = "MIK", Id = 5},
                new MaterialDto {MaterialName = "CAD-Temp mono", Id = 6},
                new MaterialDto {MaterialName = "CAD-Temp multi", Id = 7},
                new MaterialDto {MaterialName = "Enamik mono", Id = 8},
                new MaterialDto {MaterialName = "Enamik multi", Id = 9},
                new MaterialDto {MaterialName = "SUPRINITY", Id = 10},
                new MaterialDto {MaterialName = "Mark II", Id = 11},
                new MaterialDto {MaterialName = "WAX", Id = 12},
                new MaterialDto {MaterialName = "TriLuxe forte", Id = 13},
                new MaterialDto {MaterialName = "Ti", Id = 14},
                new MaterialDto {MaterialName = "E.MAX", Id = 15},
            };

            return materials;
        }

        public ProstheticsTypeDto[] GetProstheticTypes()
        {
            //Thread.Sleep(2000);
            return new ProstheticsTypeDto[]
            {
                new ProstheticsTypeDto {Name = "Каркас", Id = 1},
                new ProstheticsTypeDto {Name = "Каркас винт. фикс", Id = 2},
                new ProstheticsTypeDto {Name = "Абатмент", Id = 3},
                new ProstheticsTypeDto {Name = "Полная анатомия", Id = 4},
                new ProstheticsTypeDto {Name = "Временная конструкция", Id = 5},
                new ProstheticsTypeDto {Name = "Другая конструкция", Id = 6},
            };
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

        public void UploadStlFile(int id, byte[] stlFile)
        {
            throw new NotImplementedException();
        }

        public TransparencesDto[] GetTransparences()
        {
            return new TransparencesDto[]
            {
                new TransparencesDto {Id = 0, Name = "Мамелоны"},
                new TransparencesDto {Id = 0, Name = "Вторичный дентин"},
                new TransparencesDto {Id = 0, Name = "Зубы с сильно выраженной прозрачностью "},
                new TransparencesDto {Id = 0, Name = "Зубы со слабоо выраженной прозрачностью "},
            };
        }

        public EquipmentDto[] GetEquipments()
        {
            return GetMaterials().Select(x => new EquipmentDto { Id = x.Id, Name = x.MaterialName }).ToArray();
        }
    }
}