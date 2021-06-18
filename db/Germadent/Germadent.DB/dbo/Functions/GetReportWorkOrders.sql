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
modelWorks(WorkOrderID, ProductID, ModellerRate, Employee, EmployeePositionID) AS
(
SELECT wl.WorkOrderID, wl.ProductID, wl.Rate * wo.UrgencyRatio, CONCAT(u.FamilyName,' ', LEFT(u.FirstName, 1), '.', LEFT(u.Patronymic, 1), '.') AS Employee, teop.EmployeePositionID
FROM WorkList wl
	INNER JOIN WorkOrder wo ON wl.WorkOrderID = wo.WorkOrderID
	INNER JOIN TechnologyOperations teop ON wl.TechnologyOperationID = teop.TechnologyOperationID
	INNER JOIN Users u ON wl.EmployeeIDStarted = u.UserID
	INNER JOIN created ON wo.WorkOrderID = created.WorkOrderID
WHERE EmployeePositionID = 2
	AND created.CreateDateTime BETWEEN ISNULL(@beginningDate, '17530101') AND ISNULL(@endDate, '99991231')
),

-- Тащим работы техника
techWorks(WorkOrderID, ProductID, TechnicRate, Employee, EmployeePositionID) AS
(
SELECT wl.WorkOrderID, wl.ProductID, wl.Rate * wo.UrgencyRatio, CONCAT(u.FamilyName,' ', LEFT(u.FirstName, 1), '.', LEFT(u.Patronymic, 1), '.') AS Employee, teop.EmployeePositionID
FROM WorkList wl
	INNER JOIN WorkOrder wo ON wl.WorkOrderID = wo.WorkOrderID
	INNER JOIN TechnologyOperations teop ON wl.TechnologyOperationID = teop.TechnologyOperationID
	INNER JOIN Users u ON wl.EmployeeIDStarted = u.UserID
	INNER JOIN created ON wo.WorkOrderID = created.WorkOrderID
WHERE EmployeePositionID = 3
	AND created.CreateDateTime BETWEEN ISNULL(@beginningDate, '17530101') AND ISNULL(@endDate, '99991231')
),

-- Тащим работы оператора
operWorks(WorkOrderID, ProductID, OperatorRate, Employee, EmployeePositionID) AS
(
SELECT wl.WorkOrderID, wl.ProductID, wl.Rate * wo.UrgencyRatio, CONCAT(u.FamilyName,' ', LEFT(u.FirstName, 1), '.', LEFT(u.Patronymic, 1), '.') AS Employee, teop.EmployeePositionID
FROM WorkList wl
	INNER JOIN WorkOrder wo ON wl.WorkOrderID = wo.WorkOrderID
	INNER JOIN TechnologyOperations teop ON wl.TechnologyOperationID = teop.TechnologyOperationID
	INNER JOIN Users u ON wl.EmployeeIDStarted = u.UserID
	INNER JOIN created ON wo.WorkOrderID = created.WorkOrderID
WHERE EmployeePositionID = 4
	AND created.CreateDateTime BETWEEN ISNULL(@beginningDate, '17530101') AND ISNULL(@endDate, '99991231')
),

-- Соединяем это с основной выборкой...
ord (		Created,				DocNumber,			CustomerName, EquipmentName, PatientFullName, ProductName,	  MaterialName,		 Color, Modeller, ModellerRate, Technic, TechnicRate, Operator, OperatorRate, ImplantSystem, Cashless, Cash) AS
(
SELECT  created.CreateDateTime,	wo.DocNumber, c.CustomerName, e.EquipmentName, wo.PatientFullName, p.ProductName,  m.MaterialName, cs.Color, modelWorks.Employee, modelWorks.ModellerRate, techWorks.Employee, techWorks.TechnicRate, operWorks.Employee, operWorks.OperatorRate, ims.ImplantSystem,
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
	LEFT JOIN modelWorks ON modelWorks.WorkOrderID = wo.WorkOrderID
	LEFT JOIN techWorks ON techWorks.WorkOrderID = wo.WorkOrderID AND techWorks.ProductID = tc.ProductID
	LEFT JOIN operWorks ON operWorks.WorkOrderID = wo.WorkOrderID AND operWorks.ProductID = tc.ProductID
WHERE created.CreateDateTime BETWEEN ISNULL(@beginningDate, '17530101') AND ISNULL(@endDate, '99991231')
	AND (ae.QuantityIn > 0 OR ae.QuantityIn IS NULL)
	AND (wo.WorkOrderID IN (SELECT OrderId FROM OPENJSON (@jsonStringWOId) WITH (OrderId int)) OR @jsonStringWOId IS NULL)
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
'' E1,
ISNULL(Modeller, '') Modeller,
ISNULL(SUM(ModellerRate), 0) ModellerCost,
ISNULL(Technic, '') Technic,
ISNULL(SUM(TechnicRate), 0) TechnicCost,
ISNULL(Operator, '') Operator,
ISNULL(SUM(OperatorRate), 0) OperatorCost,
ISNULL(ImplantSystem, '') ImplantSystem,
ISNULL(SUM(Cashless), 0) Cashless, 
ISNULL(SUM(Cash), 0) Cash
FROM ord
GROUP BY Created, DocNumber, CustomerName, EquipmentName, PatientFullName, ProductName,  MaterialName, Color, Modeller, Technic, Operator, ImplantSystem

)