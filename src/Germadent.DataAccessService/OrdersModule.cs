using System;
using Germadent.Common.Logging;
using Germadent.DataAccessService.Repository;
using Germadent.Rma.Model;
using Nancy;
using Nancy.ModelBinding;

namespace Germadent.DataAccessService
{
    public class OrdersModule : NancyModule
    {
        private readonly IRmaRepository _rmaRepository;
        private readonly ILogger _logger;

        public OrdersModule(IRmaRepository rmaRepository, ILogger logger)
        {
            _rmaRepository = rmaRepository;
            _logger = logger;

            ModulePath = "api/Rma";

            Get["/orders/{id}"] = x => GetOrderById(x);
            Post["/getOrdersByFilter"] = x => GetOrdersByFilter();
            Post["/addOrder"] = x => AddOrder();
            Put["/updateOrder"] = x => UpdateOrder();

            Get["/prostheticConditions"] = x => GetProstheticConditions();
            Get["/prostheticTypes"] = x => GetProstheticTypes();
            Get["/materials"] = x => GetMaterials();
            Get["/transparences"] = x => GetTranparences();
            Get["/equipments"] = x => GetEquipments();
        }

        private object GetOrderById(dynamic arg)
        {
            return ExecuteWithLogging(() =>
            {
                var id = (int)int.Parse(arg.id.ToString());
                return Response.AsJson(_rmaRepository.GetOrderDetails(id));
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
                var order = this.Bind<OrderDto>();
                _rmaRepository.AddOrder(order);
                return Response.AsJson(order);
            });
        }

        private object UpdateOrder()
        {
            return ExecuteWithLogging(() =>
            {
                var labOrder = this.Bind<OrderDto>();
                _rmaRepository.UpdateOrder(labOrder);
                return Response.AsJson(labOrder);
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
    }
}