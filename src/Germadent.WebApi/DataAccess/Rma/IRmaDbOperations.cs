﻿using System.IO;
using Germadent.Rma.Model;
using Germadent.Rma.Model.Pricing;

namespace Germadent.WebApi.DataAccess.Rma
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
        /// Закрывает заказ-наряд по id
        /// </summary>
        /// <param name="id"></param>
        OrderDto CloseOrder(int id);

        /// <summary>
        /// Возвращает словарь
        /// </summary>
        /// <param name="dictionaryType"></param>
        /// <returns></returns>
        DictionaryItemDto[] GetDictionary(DictionaryType dictionaryType);

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
        /// Возвращает набор изделий с материалами и ценами для ценовой группы
        /// </summary>
        /// <param name="branchType"></param>
        /// <returns></returns>
        ProductDto[] GetProductSetForToothCard(BranchType branchType);
    }
}