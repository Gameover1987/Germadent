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
        OrderDto GetOrderDetails(int id);

        /// <summary>
        /// Возвращает список заказ нарядов по фильтру
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        OrderLiteDto[] GetOrders(OrdersFilter filter);

        /// <summary>
        /// Добавляет заказ наряд
        /// </summary>
        /// <param name="order"></param>
        OrderDto AddOrder(OrderDto order);

        /// <summary>
        /// Обновляет заказ наряд
        /// </summary>
        /// <param name="order"></param>
        void UpdateOrder(OrderDto order);

        /// <summary>
        /// Возвращает список условий протезирования
        /// </summary>
        /// <returns></returns>
        ProstheticConditionDto[] GetProstheticConditions();

        /// <summary>
        /// Возвращает список типов протезирования
        /// </summary>
        /// <returns></returns>
        ProstheticsTypeDto[] GetProstheticTypes();

        /// <summary>
        /// Возвращает список материалов
        /// </summary>
        /// <returns></returns>
        MaterialDto[] GetMaterials();
    }
}