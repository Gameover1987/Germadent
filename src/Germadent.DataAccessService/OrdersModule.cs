using System;
using System.Collections.Generic;
using Germadent.Rma.Model;
using Nancy;
using Nancy.ModelBinding;

namespace Germadent.DataAccessService
{
    public class OrdersModule : NancyModule
    {
        private readonly IOrdersRepository _ordersRepository;

        public OrdersModule()
        {
            _ordersRepository = new OrdersRepository();

            ModulePath = "api/Orders";

            Get("/", GetOrders);
            Post("/getOrdersByFilter", GetOrdersByFilter);
            Post("/laboratoryOrder", AddLaboratoryOrder);
            Post("/millingCenterOrder", AddMillingCenterOrder);
        }

        private object AddLaboratoryOrder(object arg)
        {
            var labOrder = this.Bind<LaboratoryOrder>();
            _ordersRepository.AddOrder(labOrder);

            return Response.AsJson("OK");
        }

        private object AddMillingCenterOrder(object arg)
        {
            var millingCenterOrder = this.Bind<MillingCenterOrder>();
            _ordersRepository.AddOrder(millingCenterOrder);

            return Response.AsJson("OK");
        }


        private object GetOrders(object arg)
        {
            return Response.AsJson(_ordersRepository.GetOrders(OrdersFilter.Empty));
        }

        private object GetOrdersByFilter(object arg)
        {
            var filter = this.Bind<OrdersFilter>();

            return Response.AsJson(_ordersRepository.GetOrders(filter));
        }
    }
}