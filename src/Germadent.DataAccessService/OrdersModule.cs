using System;
using System.Collections.Generic;
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
            _rmaRepository = new RmaRepository(new EntityToDtoConverter());
            //_rmaRepository = new RmaFileRepository();

            ModulePath = "api/Rma";
            
            Get("/orders/{id}", GetOrderById);
            Post("/getOrdersByFilter", GetOrdersByFilter);
            Post("/laboratoryOrder", AddLaboratoryOrder);
            Put("/laboratoryOrder", Action);
            Post("/millingCenterOrder", AddMillingCenterOrder);

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

        private object AddLaboratoryOrder(object arg)
        {
            var labOrder = this.Bind<LaboratoryOrder>();
            _rmaRepository.AddLabOrder(labOrder);

            return Response.AsJson(labOrder);
        }

        private object Action(object arg)
        {
            var labOrder = this.Bind<LaboratoryOrder>();
            _rmaRepository.UpdateLabOrder(labOrder);

            return Response.AsJson(labOrder);
        }

        private object AddMillingCenterOrder(object arg)
        {
            var millingCenterOrder = this.Bind<MillingCenterOrder>();
            _rmaRepository.AddMcOrder(millingCenterOrder);

            return Response.AsJson(millingCenterOrder);
        }

        private object GetMaterials(object arg)
        {
            return Response.AsJson(_rmaRepository.GetMaterials());
        }
    }
}