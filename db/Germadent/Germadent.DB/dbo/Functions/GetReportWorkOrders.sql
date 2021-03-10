-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 23.01.2021
-- Description:	Возвращает отчёт по заказ-нарядам за указанный временной промежуток
-- =============================================
CREATE FUNCTION [dbo].[GetReportWorkOrders] 
(	
	@beginningDate datetime = NULL, 
	@endDate datetime = NULL
)
RETURNS TABLE 
AS
RETURN 
(
	WITH
-- Сначала делаем выборку цветов конструкции из таблиц атрибутов...
cs (WorkOrderID, Color) AS
(
SELECT aset.WorkOrderID, av.AttributeValue AS Color
FROM dbo.WorkOrder wo 
	LEFT JOIN dbo.AttributesSet aset ON wo.WorkOrderID = aset.WorkOrderID
	INNER JOIN dbo.Attributes a ON aset.AttributeID = a.AttributeID
	INNER JOIN dbo.AttrValues av ON aset.AttributeValueID = av.AttributeValueID

WHERE wo.Created BETWEEN ISNULL(@beginningDate, '17530101') AND ISNULL(@endDate, '99991231')
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
WHERE wo.Created BETWEEN ISNULL(@beginningDate, '17530101') AND ISNULL(@endDate, '99991231')
	AND a.AttributeID = 2
),
-- Соединяем это с основной выборкой...
ord (Created, DocNumber, CustomerName, EquipmentName, PatientFullName, ProductName,  MaterialName, Color, ImplantSystem, Cashless, Cash) AS
(
SELECT wo.Created, wo.DocNumber, c.CustomerName, e.EquipmentName, wo.PatientFullName, p.ProductName,  m.MaterialName, cs.Color, ims.ImplantSystem,
	CASE wo.FlagCashless WHEN 1 THEN tc.Price END Cashless,
	CASE wo.FlagCashless WHEN 0 THEN tc.Price END Cash
FROM dbo.WorkOrder wo
	INNER JOIN dbo.Customers c ON wo.CustomerID = c.CustomerID
	INNER JOIN dbo.ToothCard tc ON wo.WorkOrderID = tc.WorkOrderID
	INNER JOIN dbo.Products p ON tc.ProductID = p.ProductID
	LEFT JOIN dbo.Materials m ON tc.MaterialID = m.MaterialID
	LEFT JOIN dbo.ResponsiblePersons rp ON wo.ResponsiblePersonID = rp.ResponsiblePersonID
	LEFT JOIN dbo.AdditionalEquipment ae ON wo.WorkOrderID = ae.WorkOrderID
	LEFT JOIN dbo.Equipments e ON ae.EquipmentID = e.EquipmentID
	LEFT JOIN cs ON cs.WorkOrderID = wo.WorkOrderID
	LEFT JOIN ims ON ims.WorkOrderID = wo.WorkOrderID
WHERE wo.Created BETWEEN ISNULL(@beginningDate, '17530101') AND ISNULL(@endDate, '99991231')
	AND (ae.QuantityIn > 0 OR ae.QuantityIn IS NULL)
)
-- ... и агрегируем
SELECT 
FORMAT(Created, 'dd.MM.yyyy HH:mm:ss') AS Created,
DocNumber, 
CustomerName, 
ISNULL(EquipmentName, '') EquipmentName,
ISNULL(PatientFullName, '') PatientFullName,
ProductName, 
ISNULL(MaterialName, '') MaterialName,
ISNULL(Color, '') Color,
COUNT(ProductName) Quantity,
'' E1, '' E2, '' E3, '' E4,
ISNULL(ImplantSystem, '') ImplantSystem,
ISNULL(SUM(Cashless), 0) Cashless, 
ISNULL(SUM(Cash), 0) Cash
FROM ord
GROUP BY Created, DocNumber, CustomerName, EquipmentName, PatientFullName, ProductName,  MaterialName, Color, ImplantSystem

)