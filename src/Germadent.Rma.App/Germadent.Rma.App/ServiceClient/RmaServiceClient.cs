using System;
using DocumentFormat.OpenXml.Drawing;
using Germadent.Common.FileSystem;
using Germadent.Common.Web;
using Germadent.Rma.App.Configuration;
using Germadent.Rma.Model;
using RestSharp;

namespace Germadent.Rma.App.ServiceClient
{
    public class RmaServiceClient : ServiceClientBase, IRmaServiceClient
    {
        private readonly IConfiguration _configuration;
        private readonly IFileManager _fileManager;

        public RmaServiceClient(IConfiguration configuration, IFileManager fileManager)
        {
            _configuration = configuration;
            _fileManager = fileManager;
        }

        public void Authorize(string user, string password)
        {

        }

        public OrderLiteDto[] GetOrders(OrdersFilter ordersFilter)
        {
            return ExecuteHttpPost<OrderLiteDto[]>(_configuration.DataServiceUrl + "/api/Rma/Orders/getByFilter", ordersFilter);
        }

        public OrderDto GetOrderById(int id)
        {
            return ExecuteHttpGet<OrderDto>(_configuration.DataServiceUrl + $"/api/Rma/orders/{id}");
        }

        public byte[] GetDataFileByWorkOrderId(int id)
        {
            var apiUrl = _configuration.DataServiceUrl + string.Format("/api/Rma/orders/fileDownload/{0}", id);
            return ExecuteFileDownload(apiUrl);
        }

        public OrderDto AddOrder(OrderDto order)
        {
            var addedOrder = ExecuteHttpPost<OrderDto>(_configuration.DataServiceUrl + "/api/Rma/orders/add", order);
            if (order.DataFileName != null)
            {
                var api = string.Format("{0}/api/Rma/orders/fileUpload/{1}/{2}", _configuration.DataServiceUrl, addedOrder.WorkOrderId, _fileManager.GetShortFileName(order.DataFileName));
                ExecuteFileUpload(api, order.DataFileName);
            }

            return addedOrder;
        }

        public OrderDto UpdateOrder(OrderDto order)
        {
            var updatedOrder =  ExecuteHttpPost<OrderDto>(_configuration.DataServiceUrl + "/api/Rma/orders/update", order);
            if (order.DataFileName != null)
            {
                var api = string.Format("{0}/api/Rma/orders/fileUpload/{1}/{2}", _configuration.DataServiceUrl, order.WorkOrderId, _fileManager.GetShortFileName(order.DataFileName));
                ExecuteFileUpload(api, order.DataFileName);
            }

            return updatedOrder;
        }

        public OrderDto CloseOrder(int id)
        {
            return ExecuteHttpDelete<OrderDto>(_configuration.DataServiceUrl + $"/api/Rma/orders/{id}");
        }

        public ReportListDto[] GetWorkReport(int id)
        {
            return ExecuteHttpGet<ReportListDto[]>(_configuration.DataServiceUrl + $"/api/Rma/reports/{id}");
        }

        public CustomerDto[] GetCustomers(string mask)
        {
            return ExecuteHttpGet<CustomerDto[]>(_configuration.DataServiceUrl + $"/api/Rma/Customers?mask={mask}");
        }

        public CustomerDto AddCustomer(CustomerDto сustomerDto)
        {
            var addedCustomer = ExecuteHttpPost<CustomerDto>(_configuration.DataServiceUrl + "/api/Rma/customers/add", сustomerDto);
            CustomerRepositoryChanged?.Invoke(this, new CustomerRepositoryChangedEventArgs(new[] { addedCustomer }, null));
            return addedCustomer;
        }

        public CustomerDto UpdateCustomer(CustomerDto customerDto)
        {
            var updatedCustomer = ExecuteHttpPost<CustomerDto>(_configuration.DataServiceUrl + "/api/Rma/customers/update", customerDto);
            CustomerRepositoryChanged?.Invoke(this, new CustomerRepositoryChangedEventArgs(new[] { updatedCustomer }, null));
            return updatedCustomer;
        }

        public CustomerDeleteResult DeleteCustomer(int customerId)
        {
            return ExecuteHttpDelete<CustomerDeleteResult>(_configuration.DataServiceUrl + $"/api/Rma/Customers/{customerId}");
        }

        public ResponsiblePersonDto[] GetResponsiblePersons()
        {
            return ExecuteHttpGet<ResponsiblePersonDto[]>(_configuration.DataServiceUrl + "/api/Rma/responsiblePersons");
        }

        public ResponsiblePersonDto AddResponsiblePerson(ResponsiblePersonDto responsiblePersonDto)
        {
            var addedResponsiblePerson = ExecuteHttpPost<ResponsiblePersonDto>(_configuration.DataServiceUrl + "/api/Rma/responsiblePersons/add", responsiblePersonDto);
            ResponsiblePersonRepositoryChanged?.Invoke(this, new ResponsiblePersonRepositoryChangedEventArgs(new[] { addedResponsiblePerson }, null));
            return addedResponsiblePerson;
        }

        public ResponsiblePersonDto UpdateResponsiblePerson(ResponsiblePersonDto responsiblePersonDto)
        {
            var updatedResponsiblePerson = ExecuteHttpPost<ResponsiblePersonDto>(_configuration.DataServiceUrl + "/api/Rma/responsiblePersons/update", responsiblePersonDto);
            ResponsiblePersonRepositoryChanged?.Invoke(this, new ResponsiblePersonRepositoryChangedEventArgs(new[] { responsiblePersonDto }, null));
            return updatedResponsiblePerson;
        }

        public ResponsiblePersonDeleteResult DeleteResponsiblePerson(int responsiblePersonId)
        {
            return ExecuteHttpDelete<ResponsiblePersonDeleteResult>(_configuration.DataServiceUrl + $"/api/Rma/responsiblePersons/{responsiblePersonId}");
        }

        public DictionaryItemDto[] GetDictionary(DictionaryType dictionaryType)
        {
            return ExecuteHttpGet<DictionaryItemDto[]>(_configuration.DataServiceUrl + $"/api/Rma/Dictionaries/{dictionaryType}");
        }

        public PriceGroupDto[] GetPrice(BranchType branchType)
        {
            return new PriceGroupDto[]
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
        }

        private PriceGroupDto GetPriceGroup1()
        {
            return new PriceGroupDto
            {
                Name = "Культевые вкладки",
                Positions = new PricePositionDto[]
                {
                    new PricePositionDto {Name = "Культевая вкладка CoCr"},
                    new PricePositionDto {Name = "Культевая вкладка разборная CoCr"},
                    new PricePositionDto {Name = "Культевая вкладка металлокерамическая"},
                    new PricePositionDto {Name = "Культевая вкладка ZrO2 VITA"},
                    new PricePositionDto {Name = "Культевая вкладка Ti"},
                }
            };
        }

        private PriceGroupDto GetPriceGroup2()
        {
            return new PriceGroupDto
            {
                Name = "Временные конструкции",
                Positions = new PricePositionDto[]
                {
                    new PricePositionDto {Name = "Временная коронка РММА"},
                    new PricePositionDto {Name = "Временная коронка РММА multicolor"},
                    new PricePositionDto {Name = "Временная коронка VITA CAD - Temp"},
                    new PricePositionDto {Name = "Временная коронка VITA CAD – Temp multicolor"},
                    new PricePositionDto {Name = "Временная коронка РММА на имплантате"},
                    new PricePositionDto {Name = "Временная коронка РММА multicolor на имплантате"},
                    new PricePositionDto {Name = "Временная коронка VITA CAD – Temp на имплантате"},
                    new PricePositionDto {Name = "Временная коронка VITA CAD – Temp multicolor на имплантате"},
                }
            };
        }

        private PriceGroupDto GetPriceGroup3()
        {
            return new PriceGroupDto
            {
                Name = "Абатменты",
                Positions = new PricePositionDto[]
                {
                    new PricePositionDto {Name = "Индивидуальный абатмент Ti (PreFace Ортос)"},
                    new PricePositionDto {Name = "Индивидуальный абатмент Ti (PreFace MEDENTiKA)"},
                    new PricePositionDto {Name = "Индивидуальный абатмент Ti (PreFace Straumann)"},
                    new PricePositionDto {Name = "Индивидуальный абатмент ZrO2 (TiBase Ортос)"},
                    new PricePositionDto {Name = "Индивидуальный абатмент ZrO2 (TiBase MEDENTiKA)"},
                    new PricePositionDto {Name = "Индивидуальный абатмент ZrO2 (TiBase Straumann)"},
                }
            };
        }

        private PriceGroupDto GetPriceGroup4()
        {
            return new PriceGroupDto
            {
                Name = "Металлокерамические конструкции",
                Positions = new PricePositionDto[]
                {
                    new PricePositionDto {Name = "Металлокерамическая коронка CAD CAM VITA VM 13"},
                    new PricePositionDto {Name = "Металлокерамическая коронка на имплантате CAD CAM VITA VM 13"},
                    new PricePositionDto {Name = "Металлокерамическая коронка на имплантате винтовая фиксация CAD CAM VITA VM 13"},
                }
            };
        }

        private PriceGroupDto GetPriceGroup5()
        {
            return new PriceGroupDto
            {
                Name = "Конструкции из ZrO2",
                Positions = new PricePositionDto[]
                {
                    new PricePositionDto {Name = "Коронка ZrO2 полная анатомия (окрашиваниe) VITA AKZENT PLUS"},
                    new PricePositionDto {Name = "Коронка ZrO2 (редуцированиe) VITA"},
                    new PricePositionDto {Name = "Коронка ZrO2 (нанесениe) VITA"},
                    new PricePositionDto {Name = "Коронка ZrO2 полная анатомия (окрашиваниe) на импланте VITA AKZENT PLUS"},
                    new PricePositionDto {Name = "Коронка ZrO2 (редуцированиe) на импланте VITA"},
                    new PricePositionDto {Name = "Коронка ZrO2 (нанесениe) на импланте VITA"},
                    new PricePositionDto {Name = "Коронка ZrO2 полная анатомия (окрашиваниe) на импланте винтовая фиксация VITA AKZENT PLUS"},
                    new PricePositionDto {Name = "Коронка ZrO2 (редуцированиe) на импланте винтовая фиксация VITA"},
                    new PricePositionDto {Name = "Коронка ZrO2 (нанесениe) на импланте винтовая фиксация VITA"},
                }
            };
        }

        private PriceGroupDto GetPriceGroup6()
        {
            return new PriceGroupDto
            {
                Name = "Дисиликат лития",
                Positions = new PricePositionDto[]
                {
                    new PricePositionDto {Name = "Вкладка/накладка,коронка/винир (окрашивание)  Коронка/винир на имплантате (окрашивание) (VITA SUPRINITY)"},
                    new PricePositionDto {Name = "Коронка/винир (редуцирование) Коронка/винир (редуцирования на имплантате) (VITA SUPRINITY)"},
                    new PricePositionDto {Name = "Вкладка/накладка,коронка/винир (окрашивание)  Коронка/винир на имплантате (окрашивание) (E-MAX)"},
                    new PricePositionDto {Name = "Коронка/винир (редуцирование) Коронка/винир (редуцирования на имплантате) (E-MAX)"}
                }
            };
        }

        private PriceGroupDto GetPriceGroup7()
        {
            return new PriceGroupDto
            {
                Name = "Полевошпатная керамика VITA",
                Positions = new PricePositionDto[]
                {
                    new PricePositionDto {Name = "Вкладка/накладка окклюзионная, коронка/винир (окрашиваниe) MARK  II"},
                    new PricePositionDto {Name = "Коронка/винир (редуцирование) MARK  II"},
                    new PricePositionDto {Name = "Коронка/винир TriLuxe forte"},
                    new PricePositionDto {Name = "Коронка/винир RealLife"}
                }
            };
        }

        private PriceGroupDto GetPriceGroup8()
        {
            return new PriceGroupDto
            {
                Name = "Полевошпатная керамика VITA",
                Positions = new PricePositionDto[]
                {
                    new PricePositionDto {Name = "Вкладка/накладка окклюзионная, Коронка/винир, Коронка/винир на имплантате VITA ENAMIC monocolor"},
                    new PricePositionDto {Name = "Вкладка/накладка окклюзионная, Коронка/винир, Коронка/винир на имплантате VITA ENAMIC multicolor"},
                }
            };
        }

        public event EventHandler<CustomerRepositoryChangedEventArgs> CustomerRepositoryChanged;
        
        public event EventHandler<ResponsiblePersonRepositoryChangedEventArgs> ResponsiblePersonRepositoryChanged;

        protected override void HandleError(IRestResponse response)
        {
            throw new ServerSideException(response);
        }
    }
}