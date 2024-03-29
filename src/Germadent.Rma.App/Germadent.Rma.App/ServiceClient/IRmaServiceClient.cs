﻿using System;
using Germadent.Client.Common.ServiceClient;
using Germadent.Model;
using Germadent.Model.Pricing;
using Germadent.Model.Production;

namespace Germadent.Rma.App.ServiceClient
{
    /// <summary>
    /// Интерфейс для взаимодействия с сервисом данных РМА
    /// </summary>
    public interface IRmaServiceClient : IBaseClientOperationsServiceClient
    {
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
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        void CloseOrder(int workOrderId);

        /// <summary>
        /// Скопировать данные заказ-наряда в буфер обмена
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ReportListDto[] GetOrdersByProducts(int id);

        /// <summary>
        /// Возвращает список заказчиков
        /// </summary>
        /// <returns></returns>
        CustomerDto[] GetCustomers(string mask);

        /// <summary>
        /// Добавление заказчика
        /// </summary>
        CustomerDto AddCustomer(CustomerDto сustomerDto);

        /// <summary>
        /// Обновляет данные заказчика
        /// </summary>
        /// <param name="customerDto"></param>
        /// <returns></returns>
        CustomerDto UpdateCustomer(CustomerDto customerDto);

        /// <summary>
        /// Удаляет заказчика по его Id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        DeleteResult DeleteCustomer(int customerId);

        /// <summary>
        /// Возвращает список ответственных лиц по Id заказчика
        /// </summary>
        /// <returns></returns>
        ResponsiblePersonDto[] GetResponsiblePersons();

        /// <summary>
        /// Добавление ответственного лица
        /// </summary>
        /// <param name="responsiblePersonDto"></param>
        /// <returns></returns>
        ResponsiblePersonDto AddResponsiblePerson(ResponsiblePersonDto responsiblePersonDto);

        /// <summary>
        /// Обновляет данные ответственного лица
        /// </summary>
        /// <param name="responsiblePersonDto"></param>
        /// <returns></returns>
        ResponsiblePersonDto UpdateResponsiblePerson(ResponsiblePersonDto responsiblePersonDto);

        /// <summary>
        /// Удаляет ответственное лицо по Id
        /// </summary>
        /// <param name="responsiblePersonId"></param>
        /// <returns></returns>
        DeleteResult DeleteResponsiblePerson(int responsiblePersonId);

        /// <summary>
        /// Возвращает прайс по выбранному типу филиала
        /// </summary>
        /// <param name="branchType"></param>
        /// <returns></returns>
        PriceGroupDto[] GetPriceGroups(BranchType branchType);

        /// <summary>
        /// Добавляет ценовую группу
        /// </summary>
        /// <param name="priceGroupDto"></param>
        /// <returns></returns>
        PriceGroupDto AddPriceGroup(PriceGroupDto priceGroupDto);

        /// <summary>
        /// Обновляет ценовую группу
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
        /// Возвращает ценовые позиции по выбранному типу филиала
        /// 
        /// </summary>
        /// <param name="branchType"></param>
        /// <returns></returns>
        PricePositionDto[] GetPricePositions(BranchType branchType);

        /// <summary>
        /// Добавляет ценовую позицию
        /// </summary>
        /// <param name="pricePositionDto"></param>
        PricePositionDto AddPricePosition(PricePositionDto pricePositionDto);

        /// <summary>
        /// Обновляет ценовую позицию
        /// </summary>
        /// <param name="pricePositionDto"></param>
        /// <returns></returns>
        PricePositionDto UpdatePricePosition(PricePositionDto pricePositionDto);

        /// <summary>
        /// Удаляет ценовую позицию
        /// </summary>
        /// <param name="pricePositionId"></param>
        /// <returns></returns>
        DeleteResult DeletePricePosition(int pricePositionId);

        /// <summary>
        /// Возвращает весь набор изделий с материалами и ценами
        /// </summary>
        /// <returns></returns>
        ProductDto[] GetProducts();

        /// <summary>
        /// Возвращает список атрибутов и их значений
        /// </summary>
        /// <returns></returns>
        AttributeDto[] GetAttributes();

        /// <summary>
        /// Возвращает список специализаций работников
        /// </summary>
        /// <returns></returns>
        EmployeePositionDto[] GetEmployeePositions();

        /// <summary>
        /// Возвращает список технологичесикх операций
        /// </summary>
        /// <returns></returns>
        TechnologyOperationDto[] GetTechnologyOperations();

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
        /// Удаляет технологическую операцию
        /// </summary>
        /// <param name="technologyOperationId"></param>
        /// <returns></returns>
        DeleteResult DeleteTechnologyOperation(int technologyOperationId);
    }
}