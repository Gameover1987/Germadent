﻿using System;
using System.Collections.Generic;
using System.Linq;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Mocks
{
    public class DesignMockRmaServiceClient : IRmaServiceClient
    {
        private readonly List<OrderDto> _orders = new List<OrderDto>();

        public DesignMockRmaServiceClient()
        {
            _orders.Add(new OrderDto
            {
                BranchType = BranchType.Laboratory,
                //Closed = DateTime.Now,
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

        public void Authorize(string user, string password)
        {
            throw new NotImplementedException();
        }

        public OrderLiteDto[] GetOrders(OrdersFilter ordersFilter = null)
        {
            //Thread.Sleep(2000);
            return _orders.Select(x => x.ToOrderLite()).ToArray();
        }

        public OrderDto GetOrderById(int id)
        {
            //Thread.Sleep(1000);
            return _orders.First(x => x.WorkOrderId == id);
        }

        public byte[] GetDataFileByWorkOrderId(int id)
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

        public void UploadStlFile(int id, byte[] stlFile)
        {
            throw new NotImplementedException();
        }

        private DictionaryItemDto[] GetTransparences()
        {
            return new DictionaryItemDto[]
            {
                new DictionaryItemDto {Id = 0, Name = "Мамелоны"},
                new DictionaryItemDto {Id = 1, Name = "Вторичный дентин"},
                new DictionaryItemDto {Id = 2, Name = "Зубы с сильно выраженной прозрачностью "},
                new DictionaryItemDto {Id = 3, Name = "Зубы со слабоо выраженной прозрачностью "},
            };
        }

        public DictionaryItemDto[] GetEquipments()
        {
            return GetMaterials().Select(x => new DictionaryItemDto { Id = x.Id, Name = x.Name }).ToArray();
        }

        public OrderDto CloseOrder(int id)
        {
            throw new NotImplementedException();
        }

        public string CopyToClipboard(OrderDto orderDto)
        {
            throw new NotImplementedException();
        }

        public string GetWorkReport(int id)
        {
            throw new NotImplementedException();
        }

        public CustomerDto[] GetCustomers(string mask)
        {
            throw new NotImplementedException();
        }

        public CustomerDto[] GetCustomers()
        {
            return new CustomerDto[]
            {
                new CustomerDto {Name = "ООО Рога и копыта", Description = "Какой то заказчик", Phone = "+7(383)222-33-45", Email = "somethingmail@mail.com",WebSite = "https://zloekino.com/movie/Barnyard"},
                new CustomerDto {Name = "ООО Пошла родимая", Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    Email = "somethingmail@mail.com",
                    WebSite = "http://xn----8sbbcdtrifnipjk4bzlpa.xn--p1ai/nashi-ob-ekty/26-ooo-poshla-rodimaya"},
            };
        }

        ReportListDto[] IRmaServiceClient.GetWorkReport(int id)
        {
            throw new NotImplementedException();
        }

        public CustomerDto UpdateCustomer(CustomerDto customerDto)
        {
            throw new NotImplementedException();
        }

        public CustomerDeleteResult DeleteCustomer(int customerId)
        {
            throw new NotImplementedException();
        }

        public ResponsiblePersonDto[] GetResponsiblePersons()
        {
            throw new NotImplementedException();
        }

        public ResponsiblePersonDto AddResponsiblePerson(ResponsiblePersonDto responsiblePersonDto)
        {
            throw new NotImplementedException();
        }

        public ResponsiblePersonDto UpdateResponsiblePerson(ResponsiblePersonDto responsiblePersonDto)
        {
            throw new NotImplementedException();
        }

        public ResponsiblePersonDeleteResult DeleteResponsiblePerson(int responsiblePersonId)
        {
            throw new NotImplementedException();
        }

        public CustomerDto AddCustomer(CustomerDto сustomerDto)
        {
            throw new NotImplementedException();
        }

        public DictionaryItemDto[] GetDictionary(DictionaryType dictionaryType)
        {
            switch (dictionaryType)
            {
                case DictionaryType.Equipment:
                    return GetEquipments();

                case DictionaryType.Material:
                    return GetMaterials();

                case DictionaryType.ProstheticCondition:
                    return GetProstheticConditions();

                case DictionaryType.ProstheticType:
                    return GetProstheticTypes();

                case DictionaryType.Transparency:
                    return GetTransparences();

                default:
                    throw new NotImplementedException("Неизвестный тип словаря");
            }
        }
        
        public event EventHandler<CustomerRepositoryChangedEventArgs> CustomerRepositoryChanged;
        public event EventHandler<ResponsiblePersonRepositoryChangedEventArgs> ResponsiblePersonRepositoryChanged;

        private DictionaryItemDto[] GetProstheticConditions()
        {
            var ptostheticsConditions = new[]
            {
                new DictionaryItemDto{Name = "Культя", Id = 1},
                new DictionaryItemDto{Name = "Имплант", Id = 2},
            };

            return ptostheticsConditions;
        }

        private DictionaryItemDto[] GetMaterials()
        {

            var materials = new[]
            {
                new DictionaryItemDto {Name = "ZrO", Id = 1},
                new DictionaryItemDto {Name = "PMMA mono", Id = 2},
                new DictionaryItemDto {Name = "PMMA multi", Id = 3},
                new DictionaryItemDto {Name = "WAX", Id = 4},
                new DictionaryItemDto {Name = "MIK", Id = 5},
                new DictionaryItemDto {Name = "CAD-Temp mono", Id = 6},
                new DictionaryItemDto {Name = "CAD-Temp multi", Id = 7},
                new DictionaryItemDto {Name = "Enamik mono", Id = 8},
                new DictionaryItemDto {Name = "Enamik multi", Id = 9},
                new DictionaryItemDto {Name = "SUPRINITY", Id = 10},
                new DictionaryItemDto {Name = "Mark II", Id = 11},
                new DictionaryItemDto {Name = "WAX", Id = 12},
                new DictionaryItemDto {Name = "TriLuxe forte", Id = 13},
                new DictionaryItemDto {Name = "Ti", Id = 14},
                new DictionaryItemDto {Name = "E.MAX", Id = 15},
            };

            return materials;
        }

        private DictionaryItemDto[] GetProstheticTypes()
        {
            //Thread.Sleep(2000);
            return new DictionaryItemDto[]
            {
                new DictionaryItemDto {Name = "Каркас", Id = 1},
                new DictionaryItemDto {Name = "Каркас винт. фикс", Id = 2},
                new DictionaryItemDto {Name = "Абатмент", Id = 3},
                new DictionaryItemDto {Name = "Полная анатомия", Id = 4},
                new DictionaryItemDto {Name = "Временная конструкция", Id = 5},
                new DictionaryItemDto {Name = "Другая конструкция", Id = 6},
            };
        }
    }
}