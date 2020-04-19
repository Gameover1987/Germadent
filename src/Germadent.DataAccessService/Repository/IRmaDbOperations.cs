using System.Collections.Generic;
using System.IO;
using Germadent.Rma.Model;

namespace Germadent.DataAccessService.Repository
{
    public interface IRmaDbOperations
    {
        /// <summary>
        /// Возвращает заказ наряд по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        OrderDto GetOrderDetails(int id);

        /// <summary>
        /// Возвращает файл по заказ наряду
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Stream GetFileByWorkOrder(int id);

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
        /// <param name="stream"></param>
        OrderDto AddOrder(OrderDto order, Stream stream);

        /// <summary>
        /// Обновляет заказ наряд
        /// </summary>
        /// <param name="order"></param>
        /// <param name="stream"></param>
        void UpdateOrder(OrderDto order, Stream stream);

        /// <summary>
        /// Закрывает заказ-наряд по id
        /// </summary>
        /// <param name="id"></param>
        OrderDto CloseOrder(int id);

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

        /// <summary>
        /// Возвращает список прозрачностей
        /// </summary>
        /// <returns></returns>
        TransparencesDto[] GetTransparences();

        /// <summary>
        /// Возвращает список оснасток
        /// </summary>
        /// <returns></returns>
        EquipmentDto[] GetEquipment();

        /// <summary>
        /// Возвращает список свойств для вставки в Excel
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ReportListDto[] GetWorkReport(int id);

        /// <summary>
        /// Возвращает список клиентов по вхождению в наименование
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        CustomerDto[] GetCustomers(string name);

        /// <summary>
        /// Возвращает спиок ответственных лиц по Id заказчика
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        ResponsiblePersonDto[] GetResponsiblePersons(int customerId);

        /// <summary>
        /// Добавляет заказчика
        /// </summary>
        /// <param name="customer"></param>
        CustomerDto AddCustomer(CustomerDto customer);
    }
}