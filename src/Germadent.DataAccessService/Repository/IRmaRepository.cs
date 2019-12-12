using Germadent.Rma.Model;

namespace Germadent.DataAccessService.Repository
{
    public interface IRmaRepository
    {
        /// <summary>
        /// Возвращает заказ наряд по ижентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Order GetOrderDetails(int id);

        /// <summary>
        /// Возвращает список заказ нарядов по фильтру
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        OrderLite[] GetOrders(OrdersFilter filter);

        /// <summary>
        /// Добавляет заказ наряд
        /// </summary>
        /// <param name="laboratoryOrder"></param>
        void AddOrder(Order laboratoryOrder);

        /// <summary>
        /// Обновляет заказ наряд
        /// </summary>
        /// <param name="laboratoryOrder"></param>
        void UpdateOrder(Order laboratoryOrder);

        /// <summary>
        /// Возвращает список материалов
        /// </summary>
        /// <returns></returns>
        Material[] GetMaterials();
    }
}