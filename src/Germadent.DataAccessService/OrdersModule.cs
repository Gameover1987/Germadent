using System;
using System.Collections.Generic;
using Germadent.DataAccessService.Configuration;
using Germadent.DataAccessService.Entities.Conversion;
using Germadent.DataAccessService.Repository;
using Germadent.Rma.Model;
using Nancy;
using Nancy.ModelBinding;

namespace Germadent.DataAccessService
{
    public class OrdersModule : NancyModule
    {
        private readonly IRmaRepository _rmaRepository;

        public OrdersModule()
        {
            _rmaRepository = new RmaRepository(new EntityToDtoConverter(), new ServiceConfiguration());

            ModulePath = "api/Rma";
            
            Get("/orders/{id}", GetOrderById);
            Post("/getOrdersByFilter", GetOrdersByFilter);
            Post("/addOrder", AddOrder);
            Put("/updateOrder", UpdateOrder);

            Get("/materials", GetMaterials);
        }

        private object GetOrderById(dynamic arg)
        {
            var id = (int)int.Parse(arg.id.ToString());
            return Response.AsJson(_rmaRepository.GetOrderDetails(id));
        }

        private object GetOrdersByFilter(object arg)
        {
            var filter = this.Bind<OrdersFilter>();

            var orders = _rmaRepository.GetOrders(filter);

            return Response.AsJson(orders);
        }

        private object AddOrder(object arg)
        {
            var labOrder = this.Bind<OrderDto>();
            _rmaRepository.AddOrder(labOrder);

            return Response.AsJson(labOrder);
        }

        private object UpdateOrder(object arg)
        {
            var labOrder = this.Bind<OrderDto>();
            _rmaRepository.UpdateOrder(labOrder);

            return Response.AsJson(labOrder);
        }

        private object GetMaterials(object arg)
        {
            return Response.AsJson(_rmaRepository.GetMaterials());
        }
    }
}