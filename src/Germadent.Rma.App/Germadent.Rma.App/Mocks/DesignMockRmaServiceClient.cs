using System;
using System.Collections.Generic;
using System.Linq;
using Germadent.Common.Extensions;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.Model;
using Germadent.Rma.Model.Pricing;
using Germadent.Rma.Model.Production;
using Germadent.UserManagementCenter.Model;
using Germadent.UserManagementCenter.Model.Rights;

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
                        ToothNumber = 11
                    },
                    new ToothDto
                    {
                        HasBridge = true,
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
                        ToothNumber = 11
                    },
                    new ToothDto
                    {
                        HasBridge = true,
                        ToothNumber = 12
                    },
                }
            });
        }

        public void Authorize(string user, string password)
        {
            throw new NotImplementedException();
        }

        public AuthorizationInfoDto AuthorizationInfo { get; }

        public RightDto[] GetRights(int userId)
        {
            throw new NotImplementedException();
        }


        public event EventHandler<UserAuthorizedEventArgs> Authorized;

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

        public DeleteResult DeleteCustomer(int customerId)
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

        public DeleteResult DeleteResponsiblePerson(int responsiblePersonId)
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

                default:
                    throw new NotImplementedException("Неизвестный тип словаря");
            }
        }

        public PriceGroupDto[] GetPriceGroups(BranchType branchType)
        {
            var groups = new PriceGroupDto[]
            {
                GetPriceGroup1(),
                GetPriceGroup2(),
                GetPriceGroup3(),
                GetPriceGroup4(),
                GetPriceGroup5(),
                GetPriceGroup6(),
                GetPriceGroup7(),
                GetPriceGroup8(),
            };
            for (int i = 0; i < groups.Length; i++)
            {
                groups[i].PriceGroupId = i;
                groups[i].BranchType = branchType;
            }
            
            return groups;
        }

        public PriceGroupDto AddPriceGroup(PriceGroupDto priceGroupDto)
        {
            throw new NotImplementedException();
        }

        public PriceGroupDto UpdatePriceGroup(PriceGroupDto priceGroupDto)
        {
            throw new NotImplementedException();
        }

        public DeleteResult DeletePriceGroup(int priceGroupId)
        {
            throw new NotImplementedException();
        }

        public PricePositionDto[] GetPricePositions(BranchType branchType)
        {
            var positions = new PricePositionDto[]
            {
                new PricePositionDto {BranchType = branchType, Name = "Культевая вкладка CoCr", UserCode = "101"},
                new PricePositionDto {BranchType = branchType, Name = "Культевая вкладка разборная CoCr", UserCode = "102"},
                new PricePositionDto {BranchType = branchType, Name = "Культевая вкладка металлокерамическая", UserCode = "103"},
                new PricePositionDto {BranchType = branchType, Name = "Культевая вкладка ZrO2 VITA", UserCode = "104"},
                new PricePositionDto {BranchType = branchType, Name = "Культевая вкладка Ti", UserCode = "105"},
            };

            return positions;
        }

        private PriceGroupDto GetPriceGroup1()
        {
            return new PriceGroupDto
            {
                Name = "Культевые вкладки",
                //Positions = new PricePositionDto[]
                //{
                //    new PricePositionDto {Name = "Культевая вкладка CoCr"},
                //    new PricePositionDto {Name = "Культевая вкладка разборная CoCr"},
                //    new PricePositionDto {Name = "Культевая вкладка металлокерамическая"},
                //    new PricePositionDto {Name = "Культевая вкладка ZrO2 VITA"},
                //    new PricePositionDto {Name = "Культевая вкладка Ti"},
                //}
            };
        }

        private PriceGroupDto GetPriceGroup2()
        {
            return new PriceGroupDto
            {
                Name = "Временные конструкции",
                //Positions = new PricePositionDto[]
                //{
                //    new PricePositionDto {Name = "Временная коронка РММА"},
                //    new PricePositionDto {Name = "Временная коронка РММА multicolor"},
                //    new PricePositionDto {Name = "Временная коронка VITA CAD - Temp"},
                //    new PricePositionDto {Name = "Временная коронка VITA CAD – Temp multicolor"},
                //    new PricePositionDto {Name = "Временная коронка РММА на имплантате"},
                //    new PricePositionDto {Name = "Временная коронка РММА multicolor на имплантате"},
                //    new PricePositionDto {Name = "Временная коронка VITA CAD – Temp на имплантате"},
                //    new PricePositionDto {Name = "Временная коронка VITA CAD – Temp multicolor на имплантате"},
                //}
            };
        }

        private PriceGroupDto GetPriceGroup3()
        {
            return new PriceGroupDto
            {
                Name = "Абатменты",
                //Positions = new PricePositionDto[]
                //{
                //    new PricePositionDto {Name = "Индивидуальный абатмент Ti (PreFace Ортос)"},
                //    new PricePositionDto {Name = "Индивидуальный абатмент Ti (PreFace MEDENTiKA)"},
                //    new PricePositionDto {Name = "Индивидуальный абатмент Ti (PreFace Straumann)"},
                //    new PricePositionDto {Name = "Индивидуальный абатмент ZrO2 (TiBase Ортос)"},
                //    new PricePositionDto {Name = "Индивидуальный абатмент ZrO2 (TiBase MEDENTiKA)"},
                //    new PricePositionDto {Name = "Индивидуальный абатмент ZrO2 (TiBase Straumann)"},
                //}
            };
        }

        private PriceGroupDto GetPriceGroup4()
        {
            return new PriceGroupDto
            {
                Name = "Металлокерамические конструкции",
                //Positions = new PricePositionDto[]
                //{
                //    new PricePositionDto {Name = "Металлокерамическая коронка CAD CAM VITA VM 13"},
                //    new PricePositionDto {Name = "Металлокерамическая коронка на имплантате CAD CAM VITA VM 13"},
                //    new PricePositionDto {Name = "Металлокерамическая коронка на имплантате винтовая фиксация CAD CAM VITA VM 13"},
                //}
            };
        }

        private PriceGroupDto GetPriceGroup5()
        {
            return new PriceGroupDto
            {
                Name = "Конструкции из ZrO2",
                //Positions = new PricePositionDto[]
                //{
                //    new PricePositionDto {Name = "Коронка ZrO2 полная анатомия (окрашиваниe) VITA AKZENT PLUS"},
                //    new PricePositionDto {Name = "Коронка ZrO2 (редуцированиe) VITA"},
                //    new PricePositionDto {Name = "Коронка ZrO2 (нанесениe) VITA"},
                //    new PricePositionDto {Name = "Коронка ZrO2 полная анатомия (окрашиваниe) на импланте VITA AKZENT PLUS"},
                //    new PricePositionDto {Name = "Коронка ZrO2 (редуцированиe) на импланте VITA"},
                //    new PricePositionDto {Name = "Коронка ZrO2 (нанесениe) на импланте VITA"},
                //    new PricePositionDto {Name = "Коронка ZrO2 полная анатомия (окрашиваниe) на импланте винтовая фиксация VITA AKZENT PLUS"},
                //    new PricePositionDto {Name = "Коронка ZrO2 (редуцированиe) на импланте винтовая фиксация VITA"},
                //    new PricePositionDto {Name = "Коронка ZrO2 (нанесениe) на импланте винтовая фиксация VITA"},
                //}
            };
        }

        private PriceGroupDto GetPriceGroup6()
        {
            return new PriceGroupDto
            {
                Name = "Дисиликат лития",
                //Positions = new PricePositionDto[]
                //{
                //    new PricePositionDto {Name = "Вкладка/накладка,коронка/винир (окрашивание)  Коронка/винир на имплантате (окрашивание) (VITA SUPRINITY)"},
                //    new PricePositionDto {Name = "Коронка/винир (редуцирование) Коронка/винир (редуцирования на имплантате) (VITA SUPRINITY)"},
                //    new PricePositionDto {Name = "Вкладка/накладка,коронка/винир (окрашивание)  Коронка/винир на имплантате (окрашивание) (E-MAX)"},
                //    new PricePositionDto {Name = "Коронка/винир (редуцирование) Коронка/винир (редуцирования на имплантате) (E-MAX)"}
                //}
            };
        }

        private PriceGroupDto GetPriceGroup7()
        {
            return new PriceGroupDto
            {
                Name = "Полевошпатная керамика VITA",
                //Positions = new PricePositionDto[]
                //{
                //    new PricePositionDto {Name = "Вкладка/накладка окклюзионная, коронка/винир (окрашиваниe) MARK  II"},
                //    new PricePositionDto {Name = "Коронка/винир (редуцирование) MARK  II"},
                //    new PricePositionDto {Name = "Коронка/винир TriLuxe forte"},
                //    new PricePositionDto {Name = "Коронка/винир RealLife"}
                //}
            };
        }

        private PriceGroupDto GetPriceGroup8()
        {
            return new PriceGroupDto
            {
                Name = "Полевошпатная керамика VITA",
                //Positions = new PricePositionDto[]
                //{
                //    new PricePositionDto {Name = "Вкладка/накладка окклюзионная, Коронка/винир, Коронка/винир на имплантате VITA ENAMIC monocolor"},
                //    new PricePositionDto {Name = "Вкладка/накладка окклюзионная, Коронка/винир, Коронка/винир на имплантате VITA ENAMIC multicolor"},
                //}
            };
        }
      

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

        public PricePositionDto AddPricePosition(PricePositionDto pricePositionDto)
        {
            throw new NotImplementedException();
        }

        public PricePositionDto UpdatePricePosition(PricePositionDto pricePositionDto)
        {
            throw new NotImplementedException();
        }

        public DeleteResult DeletePricePosition(int pricePositionId)
        {
            throw new NotImplementedException();
        }

        public ProductDto[] GetProducts()
        {
            throw new NotImplementedException();
        }

        public AttributeDto[] GetAttributes()
        {
            throw new NotImplementedException();
        }

        public EmployeePositionDto[] GetEmployeePositions()
        {
            return new EmployeePositionDto[]
            {
                new EmployeePositionDto {Name = "Администратор", EmployeePositionId = 1},
                new EmployeePositionDto {Name = "Моделировщик"},
                new EmployeePositionDto {Name = "Техник"},
                new EmployeePositionDto {Name = "Оператор"},
            };
        }

        public TechnologyOperationDto[] GetTechnologyOperations()
        {
            return new TechnologyOperationDto[]
            {
                new TechnologyOperationDto{Name = "Титановые основания ОРТОС (включая винт)", UserCode = "152", EmployeePositionId = 1, Rate = 123},
                new TechnologyOperationDto{Name = "Единица  фрезерованного каркаса на винтовой фиксации с уровня мультиюнита опорная часть (не включая винта)", UserCode = "117", EmployeePositionId = 1 , Rate = 333},
                new TechnologyOperationDto{Name = "VITA ENAMIC Monocolor", UserCode = "126", EmployeePositionId = 1, Rate = 555},
                new TechnologyOperationDto{Name = "Титановые основания ОРТОС (включая винт)", UserCode = "152", EmployeePositionId = 1, Rate = 123},
                new TechnologyOperationDto{Name = "Единица  фрезерованного каркаса на винтовой фиксации с уровня мультиюнита опорная часть (не включая винта)", UserCode = "117", EmployeePositionId = 1 , Rate = 333},
                new TechnologyOperationDto{Name = "VITA ENAMIC Monocolor", UserCode = "126", EmployeePositionId = 1, Rate = 555},
            };
        }
    }
}