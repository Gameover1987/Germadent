﻿using System.IO;
using Germadent.Model;
using Germadent.Model.Pricing;
using Germadent.Model.Production;

namespace Germadent.WebApi.DataAccess.Rma
{
    public interface IRmaDbOperations
    {
        /// <summary>
        /// Возвращает заказ наряд по идентификатору
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        OrderDto GetOrderDetails(int workOrderId, int userId);

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
        /// Закрывает заказ-наряд
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="userId"></param>
        OrderStatusNotificationDto CloseOrder(int workOrderId, int userId);

        /// <summary>
        /// Возвращает словарь
        /// </summary>
        /// <param name="dictionaryType"></param>
        /// <returns></returns>
        DictionaryItemDto[] GetDictionary(DictionaryType dictionaryType);

        /// <summary>
        /// Возвращает список свойств для вставки в Excel
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        ReportListDto[] GetOrdersByProducts(int workOrderId);

        /// <summary>
        /// Возвращает список клиентов по вхождению в наименование
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        CustomerDto[] GetCustomers(string name);

        /// <summary>
        /// Добавляет заказчика
        /// </summary>
        /// <param name="customer"></param>
        CustomerDto AddCustomer(CustomerDto customer);

        /// <summary>
        /// Обновляет данные заказчика
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        CustomerDto UpdateCustomer(CustomerDto customer);

        /// <summary>
        /// Возвращает спиок ответственных лиц по Id заказчика
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        ResponsiblePersonDto[] GetResponsiblePersons();

        /// <summary>
        /// Добавляет ответственное лицо
        /// </summary>
        /// <returns></returns>
        ResponsiblePersonDto AddResponsiblePerson(ResponsiblePersonDto responsiblePerson);

        /// <summary>
        /// Удаляет заказчика по его Id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        DeleteResult DeleteCustomer(int customerId);

        /// <summary>
        /// Обновляет данные по ответственному лицу
        /// </summary>
        /// <param name="responsiblePerson"></param>
        /// <returns></returns>
        ResponsiblePersonDto UpdateResponsiblePerson(ResponsiblePersonDto responsiblePerson);

        /// <summary>
        /// Удаляет ответственное лицо по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DeleteResult DeleteResponsiblePerson(int id);

        /// <summary>
        /// Возвращает ценовые группы по типу филиала
        /// </summary>
        /// <param name="branchType"></param>
        /// <returns></returns>
        PriceGroupDto[] GetPriceGroups(BranchType branchType);

        /// <summary>
        /// Возвращает ценовые позиции по выбранному типу филиала
        /// </summary>
        /// <param name="branchType"></param>
        /// <returns></returns>
        PricePositionDto[] GetPricePositions(BranchType branchType);

        /// <summary>
        /// Возвращает набор изделий для ценовой позиции
        /// </summary>
        /// <returns></returns>
        ProductDto[] GetProducts();

        /// <summary>
        /// Добавляет ценовую группу
        /// </summary>
        /// <param name="priceGroupDto"></param>
        /// <returns></returns>
        PriceGroupDto AddPriceGroup(PriceGroupDto priceGroupDto);

        /// <summary>
        /// Обновить ценовую группу
        /// </summary>
        /// <param name="priceGroupDto"></param>
        /// <returns></returns>
        PriceGroupDto UpdatePriceGroup(PriceGroupDto priceGroupDto);

        /// <summary>
        /// Удаляет ценовую группу
        /// </summary>
        /// <param name="priceGroupId"></param>
        /// <returns></returns>
        DeleteResult DeletePriceGroup(int priceGroupId);

        /// <summary>
        /// Добавляет ценовую позицию
        /// </summary>
        /// <param name="pricePositionDto"></param>
        /// <returns></returns>
        PricePositionDto AddPricePosition(PricePositionDto pricePositionDto);

        /// <summary>
        /// Обновить ценовую позицию
        /// </summary>
        /// <param name="pricePositionDto"></param>
        /// <returns></returns>
        PricePositionDto UpdatePricePosition(PricePositionDto pricePositionDto);

        /// <summary>
        /// Удаляет ценовую позицию
        /// </summary>
        /// <param name="priceGroupId"></param>
        /// <returns></returns>
        DeleteResult DeletePricePosition(int priceGroupId);

        /// <summary>
        /// Возвращает набор всех атрибутов и их значений
        /// </summary>
        /// <returns></returns>
        AttributeDto[] GetAllAttributesAndValues();

        /// <summary>
        /// Возвращает список должностей
        /// </summary>
        /// <returns></returns>
        EmployeePositionDto[] GetEmployeePositions();

        /// <summary>
        /// Возвращает список технологических операций
        /// </summary>
        /// <returns></returns>
        TechnologyOperationDto[] GetTechnologyOperations();

        /// <summary>
        /// Удаляет технологическую операцию
        /// </summary>
        /// <param name="technologyOperationId"></param>
        /// <returns></returns>
        DeleteResult DeleteTechnologyOperation(int technologyOperationId);

        /// <summary>
        /// Добавляет технологическую операцию
        /// </summary>
        /// <param name="technologyOperationDto"></param>
        /// <returns></returns>
        TechnologyOperationDto AddTechnologyOperation(TechnologyOperationDto technologyOperationDto);

        /// <summary>
        /// Обновляет технологическую операцию
        /// </summary>
        /// <param name="technologyOperationDto"></param>
        /// <returns></returns>
        TechnologyOperationDto UpdateTechnologyOperation(TechnologyOperationDto technologyOperationDto);

        /// <summary>
        /// Раздлокирует заказ-наряд
        /// </summary>
        /// <param name="workOrderId"></param>
        void UnlockWorkOrder(int workOrderId);

        /// <summary>
        /// Возвращает набор технологических операций по заказ-наряду, которые можно взять в работу пользователю
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        WorkDto[] GetRelevantOperations(int workOrderId, int userId);

        /// <summary>
        /// Возвращает набор технологических операций по заказ-наряду, уже взятых в работу пользователем
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        WorkDto[] GetWorksInProgressByWorkOrder(int workOrderId, int userId);

        /// <summary>
        /// Возвращает список всех работ по заказ-наряду
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        WorkDto[] GetAllWorksByWorkOrder(int workOrderId);

        /// <summary>
        /// Запускает работы по заказ-наряду
        /// </summary>
        /// <param name="works"></param>
        OrderStatusNotificationDto StartWorks(WorkDto[] works);

        /// <summary>
        /// Подтверждает выполнение работ по заказ-наряду
        /// </summary>
        /// <param name="works"></param>
        OrderStatusNotificationDto FinishWorks(WorkDto[] works);

        /// <summary>
        /// Переводит заказ-наряд в из статуса "Контроль" в cтатус "Реализация"
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        OrderStatusNotificationDto PerformQualityControl(int workOrderId, int userId);

        /// <summary>
        /// Возвращает список выполненных сотрудником работ за указанный период
        /// </summary>
        /// <param name="salaryFilter"></param>
        /// <returns></returns>
        WorkDto[] GetSalaryReport(SalaryFilter salaryFilter);
    }
}