-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 23.01.2021
-- Description:	Возвращает отчёт по заказ-нарядам за указанный временной промежуток
-- =============================================
CREATE FUNCTION [dbo].[GetReportWorkOrders] 
(	
	@beginningDate datetime = NULL, 
	@endDate datetime = NULL,
	@jsonStringWOId varchar(max) = NULL
)
RETURNS TABLE 
AS
RETURN 
(
	WITH
-- По таблице статусов смотрим инфу о создании заказ-наряда
created (WorkOrderID, CreateDateTime, CreatorID) AS 
(
SELECT WorkOrderID, StatusChangeDateTime, UserID
FROM dbo.StatusList
WHERE Status = 0
),

-- Делаем выборку цветов конструкции из таблиц атрибутов...
cs (WorkOrderID, Color) AS
(
SELECT aset.WorkOrderID, av.AttributeValue AS Color
FROM dbo.WorkOrder wo 
	LEFT JOIN dbo.AttributesSet aset ON wo.WorkOrderID = aset.WorkOrderID
	INNER JOIN dbo.Attributes a ON aset.AttributeID = a.AttributeID
	INNER JOIN dbo.AttrValues av ON aset.AttributeValueID = av.AttributeValueID
	INNER JOIN created ON wo.WorkOrderID = created.WorkOrderID

WHERE created.CreateDateTime BETWEEN ISNULL(@beginningDate, '17530101') AND ISNULL(@endDate, '99991231')
	AND a.AttributeID = 1
),
-- ... затем - то же самое для систем имплантов
ims (WorkOrderID, ImplantSystem) AS
(
SELECT aset.WorkOrderID, av.AttributeValue AS ImplantSystem
FROM dbo.WorkOrder wo 
	LEFT JOIN dbo.AttributesSet aset ON wo.WorkOrderID = aset.WorkOrderID
	INNER JOIN dbo.Attributes a ON aset.AttributeID = a.AttributeID
	INNER JOIN dbo.AttrValues av ON aset.AttributeValueID = av.AttributeValueID
	INNER JOIN created ON wo.WorkOrderID = created.WorkOrderID
WHERE created.CreateDateTime BETWEEN ISNULL(@beginningDate, '17530101') AND ISNULL(@endDate, '99991231')
	AND a.AttributeID = 2
),

-- Тащим работы моделировщика
modelWorks(WorkOrderID, ProductID, ModellerCost, EmployeePositionID) AS
(
SELECT wl.WorkOrderID, wl.ProductID, SUM(wl.OperationCost), teop.EmployeePositionID
FROM WorkList wl
	INNER JOIN WorkOrder wo ON wl.WorkOrderID = wo.WorkOrderID
	INNER JOIN TechnologyOperations teop ON wl.TechnologyOperationID = teop.TechnologyOperationID
	INNER JOIN created ON wo.WorkOrderID = created.WorkOrderID
WHERE EmployeePositionID = 2
	AND created.CreateDateTime BETWEEN ISNULL(@beginningDate, '17530101') AND ISNULL(@endDate, '99991231')
GROUP BY wl.WorkOrderID, wl.ProductID, teop.EmployeePositionID
),

-- Тащим работы техника
techWorks(WorkOrderID, ProductID, TechnicCost, EmployeePositionID) AS
(
SELECT wl.WorkOrderID, wl.ProductID, SUM(wl.OperationCost), teop.EmployeePositionID
FROM WorkList wl
	INNER JOIN WorkOrder wo ON wl.WorkOrderID = wo.WorkOrderID
	INNER JOIN TechnologyOperations teop ON wl.TechnologyOperationID = teop.TechnologyOperationID
	INNER JOIN created ON wo.WorkOrderID = created.WorkOrderID
WHERE EmployeePositionID = 3
	AND created.CreateDateTime BETWEEN ISNULL(@beginningDate, '17530101') AND ISNULL(@endDate, '99991231')
GROUP BY wl.WorkOrderID, wl.ProductID, teop.EmployeePositionID
),

-- Тащим работы оператора
operWorks(WorkOrderID, ProductID, OperatorCost, EmployeePositionID) AS
(
SELECT wl.WorkOrderID, wl.ProductID, SUM(wl.OperationCost), teop.EmployeePositionID
FROM WorkList wl
	INNER JOIN WorkOrder wo ON wl.WorkOrderID = wo.WorkOrderID
	INNER JOIN TechnologyOperations teop ON wl.TechnologyOperationID = teop.TechnologyOperationID
	INNER JOIN created ON wo.WorkOrderID = created.WorkOrderID
WHERE EmployeePositionID = 4
	AND created.CreateDateTime BETWEEN ISNULL(@beginningDate, '17530101') AND ISNULL(@endDate, '99991231')
GROUP BY wl.WorkOrderID, wl.ProductID, teop.EmployeePositionID
),

-- Соединяем это с основной выборкой...
ord (		Created,				WorkOrderID,		DocNumber,		CustomerName, EquipmentName, PatientFullName, ProductID, ProductName,	  MaterialName,		 Color, ImplantSystem, Cashless, Cash) AS
(
SELECT  created.CreateDateTime, wo.WorkOrderID, 	wo.DocNumber, c.CustomerName, e.EquipmentName, wo.PatientFullName, p.ProductID, p.ProductName,  m.MaterialName, cs.Color, ims.ImplantSystem,
	CASE wo.FlagCashless WHEN 1 THEN tc.Price END Cashless,
	CASE wo.FlagCashless WHEN 0 THEN tc.Price END Cash
FROM dbo.WorkOrder wo
	INNER JOIN dbo.Customers c ON wo.CustomerID = c.CustomerID
	INNER JOIN dbo.ToothCard tc ON wo.WorkOrderID = tc.WorkOrderID
	INNER JOIN dbo.Products p ON tc.ProductID = p.ProductID
	INNER JOIN created ON wo.WorkOrderID = created.WorkOrderID
	LEFT JOIN dbo.Materials m ON tc.MaterialID = m.MaterialID
	LEFT JOIN dbo.ResponsiblePersons rp ON wo.ResponsiblePersonID = rp.ResponsiblePersonID
	LEFT JOIN dbo.AdditionalEquipment ae ON wo.WorkOrderID = ae.WorkOrderID
	LEFT JOIN dbo.Equipments e ON ae.EquipmentID = e.EquipmentID
	LEFT JOIN cs ON cs.WorkOrderID = wo.WorkOrderID
	LEFT JOIN ims ON ims.WorkOrderID = wo.WorkOrderID
WHERE created.CreateDateTime BETWEEN ISNULL(@beginningDate, '17530101') AND ISNULL(@endDate, '99991231')
	AND (ae.QuantityIn > 0 OR ae.QuantityIn IS NULL)
	AND (wo.WorkOrderID IN (SELECT OrderId FROM OPENJSON (@jsonStringWOId) WITH (OrderId int)) OR @jsonStringWOId IS NULL)
),
-- ... и агрегируем
ordgrp (Created, WorkOrderID, DocNumber, CustomerName, EquipmentName, PatientFullName, ProductID, ProductName, MaterialName, Color, Quantity, ImplantSystem, Cashless, Cash) AS 
(SELECT Created, WorkOrderID, DocNumber, CustomerName, EquipmentName, PatientFullName, ProductID, ProductName, MaterialName, Color, COUNT(ProductName), ImplantSystem, SUM(Cashless) Cashless, SUM(Cash) Cash
FROM ord
GROUP BY Created, WorkOrderID, DocNumber, CustomerName, EquipmentName, PatientFullName, ProductID, ProductName,  MaterialName, Color, ImplantSystem)

-- Цепляем зарплату
SELECT FORMAT(Created, 'dd.MM.yyyy HH:mm:ss') Created, 
	DocNumber, 
	CustomerName, 
	ISNULL(EquipmentName, '') EquipmentName, 
	ISNULL(PatientFullName, '') PatientFullName,
	ProductName,
	ISNULL(MaterialName, '') MaterialName, 
	ISNULL(Color, '') Color, 
	Quantity, 
	ISNULL(ModellerCost, 0) ModellerCost, 
	ISNULL(TechnicCost, 0) TechnicCost, 
	ISNULL(OperatorCost, 0) OperatorCost, 
	ISNULL(ImplantSystem, '') ImplantSystem, 
	ISNULL(Cashless, 0) Cashless,
	ISNULL(Cash, 0) Cash
FROM ordgrp
	LEFT JOIN modelWorks ON modelWorks.WorkOrderID = ordgrp.WorkOrderID AND modelWorks.ProductID = ordgrp.ProductID
	LEFT JOIN techWorks ON techWorks.WorkOrderID = ordgrp.WorkOrderID AND techWorks.ProductID = ordgrp.ProductID
	LEFT JOIN operWorks ON operWorks.WorkOrderID = ordgrp.WorkOrderID AND operWorks.ProductID = ordgrp.ProductID
)