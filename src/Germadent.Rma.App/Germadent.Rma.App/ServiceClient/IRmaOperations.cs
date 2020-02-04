using Germadent.Rma.Model;

namespace Germadent.Rma.App.ServiceClient
{
    /// <summary>
    /// Интерфейс для взаимодействия с сервисом данных РМА
    /// </summary>
    public interface IRmaOperations
    {
        /// <summary>
        /// Получить список заказнарядов
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        OrderLiteDto[] GetOrders(OrdersFilter filter = null);

        /// <summary>
        /// Поулчить детали по выбранному заказнаряду
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        OrderDto GetOrderDetails(int id);

        /// <summary>
        /// Получить список условий протезирования
        /// </summary>
        /// <returns></returns>
        ProstheticConditionDto[] GetProstheticConditions();

        /// <summary>
        /// Получить список типов протезирования
        /// </summary>
        /// <returns></returns>
        ProstheticsTypeDto[] GetProstheticTypes();

        /// <summary>
        /// Получить список материалов
        /// </summary>
        /// <returns></returns>
        MaterialDto[] GetMaterials();

        /// <summary>
        /// Добавить заказнаряд
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        OrderDto AddOrder(OrderDto order);

        /// <summary>
        /// Обновить заказнаряд
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        OrderDto UpdateOrder(OrderDto order);
    }
}