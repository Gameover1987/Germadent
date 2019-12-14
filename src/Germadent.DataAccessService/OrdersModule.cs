using Germadent.DataAccessService.Repository;
using Germadent.Rma.Model;
using Nancy;
using Nancy.ModelBinding;

namespace Germadent.DataAccessService
{
    public class OrdersModule : NancyModule
    {
        private readonly IRmaRepository _rmaRepository;

        public OrdersModule(IRmaRepository rmaRepository)
        {
            _rmaRepository = rmaRepository;

            ModulePath = "api/Rma";

            Get["/orders/{id}"] = x => GetOrderById(x);
            Post["/getOrdersByFilter"] = x => GetOrdersByFilter();
            Post["/addOrder"] = x => AddOrder();
            Put["/updateOrder"] = x => UpdateOrder();

            Get["/materials"] = x => GetMaterials();
        }

        private object GetOrderById(dynamic arg)
        {
            var id = (int)int.Parse(arg.id.ToString());
            return Response.AsJson(_rmaRepository.GetOrderDetails(id));
        }

        private object GetOrdersByFilter()
        {
            var filter = this.Bind<OrdersFilter>();

            var orders = _rmaRepository.GetOrders(filter);

            return Response.AsJson(orders);
        }

        private object AddOrder()
        {
            var order = this.Bind<OrderDto>();
            _rmaRepository.AddOrder(order);

            return Response.AsJson(order);
        }

        private object UpdateOrder()
        {
            var labOrder = this.Bind<OrderDto>();
            _rmaRepository.UpdateOrder(labOrder);

            return Response.AsJson(labOrder);
        }

        private object GetMaterials()
        {
            return Response.AsJson(_rmaRepository.GetMaterials());
        }
    }
}