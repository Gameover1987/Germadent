﻿-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 23.01.2021
-- Description:	Возвращает отчёт по заказ-нарядам за указанный временной промежуток
-- =============================================
CREATE FUNCTION [dbo].[GetReportWorkOrders] 
(	
	@dateFrom datetime = NULL, 
	@dateTo datetime = NULL,
	@jsonStringWOId varchar(max) = NULL,
	@workOrderId int
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
cs AS
(SELECT aset.WorkOrderID, av.AttributeValue AS Color
FROM dbo.WorkOrder wo 
	LEFT JOIN dbo.AttributesSet aset ON wo.WorkOrderID = aset.WorkOrderID
	INNER JOIN dbo.Attributes a ON aset.AttributeID = a.AttributeID
	INNER JOIN dbo.AttrValues av ON aset.AttributeValueID = av.AttributeValueID
	INNER JOIN created ON wo.WorkOrderID = created.WorkOrderID

WHERE created.CreateDateTime BETWEEN ISNULL(@dateFrom, '17530101') AND ISNULL(@dateTo, '99991231')
	AND a.AttributeID = 1
),
-- ... затем - то же самое для систем имплантов
ims AS
(SELECT aset.WorkOrderID, av.AttributeValue AS ImplantSystem
FROM dbo.WorkOrder wo 
	LEFT JOIN dbo.AttributesSet aset ON wo.WorkOrderID = aset.WorkOrderID
	INNER JOIN dbo.Attributes a ON aset.AttributeID = a.AttributeID
	INNER JOIN dbo.AttrValues av ON aset.AttributeValueID = av.AttributeValueID
	INNER JOIN created ON wo.WorkOrderID = created.WorkOrderID
WHERE created.CreateDateTime BETWEEN ISNULL(@dateFrom, '17530101') AND ISNULL(@dateTo, '99991231')
	AND a.AttributeID = 2
),

-- Соединяем это с основной выборкой...
ord  AS
(SELECT  created.CreateDateTime AS Created, wo.WorkOrderID, wo.DocNumber, c.CustomerName, rp.ResponsiblePerson, e.EquipmentName, wo.PatientFullName, pp.PricePositionCode, p.ProductID, p.ProductName,  m.MaterialName, cs.Color, ims.ImplantSystem,
	CASE wo.FlagCashless WHEN 1 THEN tc.Price END Cashless,
	CASE wo.FlagCashless WHEN 0 THEN tc.Price END Cash
	
FROM dbo.WorkOrder wo
	INNER JOIN dbo.Customers c ON wo.CustomerID = c.CustomerID
	INNER JOIN dbo.ToothCard tc ON wo.WorkOrderID = tc.WorkOrderID
	INNER JOIN dbo.Products p ON tc.ProductID = p.ProductID
	INNER JOIN dbo.PricePositions pp ON tc.PricePositionID = pp.PricePositionID
	INNER JOIN created ON wo.WorkOrderID = created.WorkOrderID
	LEFT JOIN dbo.Materials m ON tc.MaterialID = m.MaterialID
	LEFT JOIN dbo.ResponsiblePersons rp ON wo.ResponsiblePersonID = rp.ResponsiblePersonID
	LEFT JOIN dbo.AdditionalEquipment ae ON wo.WorkOrderID = ae.WorkOrderID
	LEFT JOIN dbo.Equipments e ON ae.EquipmentID = e.EquipmentID
	LEFT JOIN cs ON cs.WorkOrderID = wo.WorkOrderID
	LEFT JOIN ims ON ims.WorkOrderID = wo.WorkOrderID
WHERE created.CreateDateTime BETWEEN ISNULL(@dateFrom, '17530101') AND ISNULL(@dateTo, '99991231')
	AND (ae.QuantityIn > 0 OR ae.QuantityIn IS NULL)
	AND (e.EquipmentName IN ('STL', 'Модель', 'Слепок') OR e.EquipmentID IS NULL)
	AND (wo.WorkOrderID IN (SELECT OrderId FROM OPENJSON (@jsonStringWOId) WITH (OrderId int)) OR @jsonStringWOId IS NULL)
	AND wo.WorkOrderID = @workOrderId
)

-- ... и агрегируем
SELECT FORMAT(Created, 'dd.MM.yyyy HH:mm:ss') Created, 
	DocNumber, 
	CustomerName,	
	ISNULL(EquipmentName, '') AS EquipmentName, 
	ISNULL(PatientFullName, '') AS PatientFullName,	
	ProductName,
	PricePositionCode,
	ISNULL(MaterialName, '') AS MaterialName, 
	ISNULL(Color, '') AS Color, 
	COUNT(ProductName) AS Quantity,
	ISNULL(ImplantSystem, '') AS ImplantSystem, 
	ISNULL(SUM(Cashless), 0) AS Cashless, 
	ISNULL(SUM(Cash), 0) AS Cash,
	ISNULL(ResponsiblePerson, '') AS ResponsiblePerson

FROM ord
GROUP BY Created, DocNumber, CustomerName, EquipmentName, PatientFullName, ProductName, PricePositionCode,  MaterialName, Color, ImplantSystem, ResponsiblePerson
)