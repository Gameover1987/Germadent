-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 23.01.2021
-- Description:	Возвращает отчёт по заказ-нарядам за указанный временной промежуток
-- =============================================
CREATE FUNCTION GetReportWorkOrders 
(	
	@beginningDate datetime, 
	@endDate datetime = NULL
)
RETURNS TABLE 
AS
RETURN 
(
	WITH
cs (WorkOrderID, Color) AS
(
SELECT aset.WorkOrderID, av.AttributeValue AS Color
FROM WorkOrder wo 
	LEFT JOIN AttributesSet aset ON wo.WorkOrderID = aset.WorkOrderID
	INNER JOIN Attributes a ON aset.AttributeID = a.AttributeID
	INNER JOIN AttrValues av ON aset.AttributeValueID = av.AttributeValueID

WHERE wo.Created BETWEEN @beginningDate AND ISNULL(@endDate, '99991231')
	AND a.AttributeID = 1
),

ims (WorkOrderID, ImplantSystem) AS
(
SELECT aset.WorkOrderID, av.AttributeValue AS ImplantSystem
FROM WorkOrder wo 
	LEFT JOIN AttributesSet aset ON wo.WorkOrderID = aset.WorkOrderID
	INNER JOIN Attributes a ON aset.AttributeID = a.AttributeID
	INNER JOIN AttrValues av ON aset.AttributeValueID = av.AttributeValueID
WHERE wo.Created BETWEEN @beginningDate AND ISNULL(@endDate, '99991231')
	AND a.AttributeID = 2
),

ord (Created, DocNumber, CustomerName, EquipmentName, PatientFullName, ProductName,  MaterialName, Color, ImplantSystem, Cashless, Cash) AS
(
SELECT wo.Created, wo.DocNumber, c.CustomerName, e.EquipmentName, wo.PatientFullName, p.ProductName,  m.MaterialName, cs.Color, ims.ImplantSystem,
	CASE wo.FlagCashless WHEN 1 THEN tc.Price END Cashless,
	CASE wo.FlagCashless WHEN 0 THEN tc.Price END Cash
FROM WorkOrder wo
	INNER JOIN Customers c ON wo.CustomerID = c.CustomerID
	INNER JOIN ToothCard tc ON wo.WorkOrderID = tc.WorkOrderID
	INNER JOIN Products p ON tc.ProductID = p.ProductID
	LEFT JOIN Materials m ON tc.MaterialID = m.MaterialID
	LEFT JOIN ResponsiblePersons rp ON wo.ResponsiblePersonID = rp.ResponsiblePersonID
	LEFT JOIN AdditionalEquipment ae ON wo.WorkOrderID = ae.WorkOrderID
	LEFT JOIN Equipments e ON ae.EquipmentID = e.EquipmentID
	LEFT JOIN cs ON cs.WorkOrderID = wo.WorkOrderID
	LEFT JOIN ims ON ims.WorkOrderID = wo.WorkOrderID
WHERE wo.Created BETWEEN @beginningDate AND ISNULL(@endDate, '99991231')
	AND (ae.QuantityIn > 0 OR ae.QuantityIn IS NULL)
)

SELECT 
Created, 
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