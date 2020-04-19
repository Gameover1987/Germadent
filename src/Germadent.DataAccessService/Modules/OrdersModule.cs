using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Germadent.Common.Extensions;
using Germadent.Common.Logging;
using Germadent.DataAccessService.Repository;
using Germadent.Rma.Model;
using Nancy;
using Nancy.ModelBinding;

namespace Germadent.DataAccessService.Modules
{
    public class OrdersModule : NancyModule
    {
        private readonly IRmaDbOperations _rmaRepository;
        private readonly ILogger _logger;

        public OrdersModule(IRmaDbOperations rmaRepository, ILogger logger)
        {
            _rmaRepository = rmaRepository;
            _logger = logger;

            ModulePath = "api/Rma";

            Get["/orders/{id}"] = x => GetOrderById(x);
            Get["/files/{id}"] = x => GetFileByWorkOrderId(x);
            Post["/getOrdersByFilter"] = x => GetOrdersByFilter();
            Post["/addOrder"] = x => AddOrder();
            Post["/updateOrder"] = x => UpdateOrder();
            Get["/closeOrder/{id}"] = x => CloseOrder(x);

            Get["/prostheticConditions"] = x => GetProstheticConditions();
            Get["/prostheticTypes"] = x => GetProstheticTypes();
            Get["/materials"] = x => GetMaterials();
            Get["/transparences"] = x => GetTranparences();
            Get["/equipments"] = x => GetEquipments();
            Get["/excel/{id}"] = x => GetExcel(x);
            Get["/customers"] = x => GetAllCustomers();
            Get["/customers/{name}"] = x => GetCustomers(x);
            Post["/addCustomer"] = x => AddCustomer();
            Get["/responsiblePersons/{customerId}"] = x => GetResponsiblePersons(x);
        }

        private object GetOrderById(dynamic arg)
        {
            return ExecuteWithLogging(() =>
            {
                var id = (int)int.Parse(arg.id.ToString());
                return Response.AsJson(_rmaRepository.GetOrderDetails(id));
            });
        }

        private object GetFileByWorkOrderId(dynamic arg)
        {
            return ExecuteWithLogging(() =>
            {
                var id = (int)int.Parse(arg.id.ToString());

                var file = _rmaRepository.GetFileByWorkOrder(id);

                var response = new Response();
                response.ContentType = "text/plain";
                response.Contents = stream =>
                {
                    file.CopyTo(stream);
                    file.Dispose();
                };
                return response;
            });
        }

        private object GetOrdersByFilter()
        {
            return ExecuteWithLogging(() =>
            {
                var filter = this.Bind<OrdersFilter>();
                var orders = _rmaRepository.GetOrders(filter);
                return Response.AsJson(orders);
            });
        }

        private object AddOrder()
        {
            return ExecuteWithLogging(() =>
            {
                var order = GetFromRequestBody<OrderDto>();
                var stream = GetFileFromRequest();
                _rmaRepository.AddOrder(order, stream);
                return Response.AsJson(order);
            });
        }

        private object UpdateOrder()
        {
            return ExecuteWithLogging(() =>
            {
                var order = GetFromRequestBody<OrderDto>();
                var stream = GetFileFromRequest();
                _rmaRepository.UpdateOrder(order, stream);
                return Response.AsJson(order);
            });
        }
        private object CloseOrder(dynamic arg)
        {
            return ExecuteWithLogging(() =>
            {
                var id = (int)int.Parse(arg.id.ToString());
                var order =_rmaRepository.CloseOrder(id);
                return Response.AsJson(order);
            });
        }

        private object GetExcel(dynamic arg)
        {
            return ExecuteWithLogging(() =>
            {
                var id = (int)int.Parse(arg.id.ToString());
                return Response.AsJson(_rmaRepository.GetWorkReport(id));
            });
        }

        private object GetCustomers(dynamic arg)
        {
            return ExecuteWithLogging(() =>
            {
                var name = (string)arg.name.ToString();
                var customers = _rmaRepository.GetCustomers(name);
                return Response.AsJson(customers);
            });
        }

        private object GetAllCustomers()
        {
            return ExecuteWithLogging(() =>
            {
                var customers = _rmaRepository.GetCustomers(string.Empty);
                return Response.AsJson(customers);
            });
        }

        private object GetResponsiblePersons(dynamic arg)
        {
            return ExecuteWithLogging(() =>
            {
                var customerId = (int)int.Parse(arg.customerId.ToString());
                var responsiblePersons = _rmaRepository.GetResponsiblePersons(customerId);
                return Response.AsJson(responsiblePersons);
            });
        }

        private object GetProstheticConditions()
        {
            return ExecuteWithLogging(() => { return Response.AsJson(_rmaRepository.GetProstheticConditions()); });
        }

        private object GetProstheticTypes()
        {
            return ExecuteWithLogging(() => { return Response.AsJson(_rmaRepository.GetProstheticTypes()); });
        }

        private object GetMaterials()
        {
            return ExecuteWithLogging(() => { return Response.AsJson(_rmaRepository.GetMaterials()); });
        }
        private object GetTranparences()
        {
            return ExecuteWithLogging(() => { return Response.AsJson(_rmaRepository.GetTransparences()); });
        }

        private object GetEquipments()
        {
            return ExecuteWithLogging(() => { return Response.AsJson(_rmaRepository.GetEquipment()); });
        }

        private T GetFromRequestBody<T>()
        {
            var contentTypeRegex = new Regex("^multipart/form-data;\\s*boundary=(.*)$", RegexOptions.IgnoreCase);
            Stream bodyStream;

            if (contentTypeRegex.IsMatch(Request.Headers.ContentType))
            {
                var boundary = contentTypeRegex.Match(Request.Headers.ContentType).Groups[1].Value;
                var multipart = new HttpMultipart(Request.Body, boundary);
                bodyStream = multipart.GetBoundaries().First(b => b.ContentType.Equals("application/json")).Value;
            }
            else
            {
                // Regular model binding goes here.
                bodyStream = Request.Body;
            }

            var jsonBody = new StreamReader(bodyStream).ReadToEnd();
            bodyStream.Close();

            var obj = jsonBody.DeserializeFromJson<T>();
            return obj;
        }

        private Stream GetFileFromRequest()
        {
            if (!Request.Files.Any())
                return null;

            return Request.Files.First().Value;
        }

        private object ExecuteWithLogging(Func<object> func)
        {
            try
            {
                return func();
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
            }

            return null;
        }

        private object AddCustomer()
        {
            return ExecuteWithLogging(() =>
            {
                var customer = GetFromRequestBody<CustomerDto>();
                _rmaRepository.AddCustomer(customer);
                return Response.AsJson(customer);
            });
        }
    }
}