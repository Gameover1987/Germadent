using Germadent.Rma.Model;

namespace Germadent.Rma.App.ServiceClient
{
    public enum DictionaryType
    {
        ProstheticCondition,
        ProstheticTypes,
        Materials,
        Transparences,
        Equipments
    }

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
        OrderLiteDto[] GetOrders(OrdersFilter filter);

        /// <summary>
        /// Получить детали по выбранному заказнаряду
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        OrderDto GetOrderById(int id);

        /// <summary>
        /// Возвращает файл по заказ наряду
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFileResponse GetDataFileByWorkOrderId(int id);

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
        /// Обновить заказ-наряд
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        OrderDto UpdateOrder(OrderDto order);

        /// <summary>
        /// Закрыть заказ-наряд
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        OrderDto CloseOrder(int id);

        /// <summary>
        /// Получить список прозрачностей
        /// </summary>
        /// <returns></returns>
        TransparencesDto[] GetTransparences();

        /// <summary>
        /// Получить список оснасток
        /// </summary>
        /// <returns></returns>
        EquipmentDto[] GetEquipments();

        /// <summary>
        /// Скопировать данные заказ-наряда в буфер обмена
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ReportListDto[] GetWorkReport(int id);

        /// <summary>
        /// Возвращает список заказчиков
        /// </summary>
        /// <returns></returns>
        CustomerDto[] GetCustomers(string mask);

        /// <summary>
        /// Возвращает список ответственных лиц по Id заказчика
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        ResponsiblePersonDto[] GetResponsiblePersons(int customerId);

        /// <summary>
        /// Добавление заказчика
        /// </summary>
        CustomerDto AddCustomer(CustomerDto сustomerDto);
    }
}