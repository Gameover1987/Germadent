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
cs AS
(SELECT aset.WorkOrderID, av.AttributeValue AS Color
FROM dbo.WorkOrder wo 
	LEFT JOIN dbo.AttributesSet aset ON wo.WorkOrderID = aset.WorkOrderID
	INNER JOIN dbo.Attributes a ON aset.AttributeID = a.AttributeID
	INNER JOIN dbo.AttrValues av ON aset.AttributeValueID = av.AttributeValueID
	INNER JOIN created ON wo.WorkOrderID = created.WorkOrderID

WHERE created.CreateDateTime BETWEEN ISNULL(@beginningDate, '17530101') AND ISNULL(@endDate, '99991231')
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
WHERE created.CreateDateTime BETWEEN ISNULL(@beginningDate, '17530101') AND ISNULL(@endDate, '99991231')
	AND a.AttributeID = 2
),

-- Находим моделировщиков
modellers AS
(SELECT wl.WorkOrderID, wl.ProductID, CONCAT(u.FamilyName,' ', LEFT(u.FirstName, 1), '.', LEFT(u.Patronymic, 1), '.') AS UserFullName
FROM dbo.WorkList wl INNER JOIN dbo.TechnologyOperations teop on wl.TechnologyOperationID = teop.TechnologyOperationID AND teop.EmployeePositionID = 2
	INNER JOIN dbo.Users u on wl.EmployeeIDStarted = u.UserID
),

-- Находим техников
technicians AS
(SELECT wl.WorkOrderID, wl.ProductID, CONCAT(u.FamilyName,' ', LEFT(u.FirstName, 1), '.', LEFT(u.Patronymic, 1), '.') AS UserFullName
FROM dbo.WorkList wl INNER JOIN dbo.TechnologyOperations teop on wl.TechnologyOperationID = teop.TechnologyOperationID AND teop.EmployeePositionID = 3
	INNER JOIN dbo.Users u on wl.EmployeeIDStarted = u.UserID
),

-- Находим операторов
operators AS
(SELECT wl.WorkOrderID, wl.ProductID, CONCAT(u.FamilyName,' ', LEFT(u.FirstName, 1), '.', LEFT(u.Patronymic, 1), '.') AS UserFullName
FROM dbo.WorkList wl INNER JOIN dbo.TechnologyOperations teop on wl.TechnologyOperationID = teop.TechnologyOperationID AND teop.EmployeePositionID = 4
	INNER JOIN dbo.Users u on wl.EmployeeIDStarted = u.UserID
),

-- Соединяем это с основной выборкой...
ord  AS
(SELECT  created.CreateDateTime AS Created, wo.WorkOrderID, wo.DocNumber, c.CustomerName, rp.ResponsiblePerson, e.EquipmentName, wo.PatientFullName, pp.PricePositionCode, p.ProductID, p.ProductName,  m.MaterialName, modellers.UserFullName AS Modeller, technicians.UserFullName AS Technician, operators.UserFullName AS Operator, cs.Color, ims.ImplantSystem,
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
	LEFT JOIN modellers ON wo.WorkOrderID = modellers.WorkOrderID AND p.ProductID = modellers.ProductID
	LEFT JOIN technicians ON wo.WorkOrderID = technicians.WorkOrderID AND p.ProductID = technicians.ProductID
	LEFT JOIN operators ON wo.WorkOrderID = operators.WorkOrderID AND p.ProductID = operators.ProductID
WHERE created.CreateDateTime BETWEEN ISNULL(@beginningDate, '17530101') AND ISNULL(@endDate, '99991231')
	AND (ae.QuantityIn > 0 OR ae.QuantityIn IS NULL)
	AND (e.EquipmentName IN ('STL', 'Модель', 'Слепок') OR e.EquipmentID IS NULL)
	AND (wo.WorkOrderID IN (SELECT OrderId FROM OPENJSON (@jsonStringWOId) WITH (OrderId int)) OR @jsonStringWOId IS NULL)
)

-- ... и агрегируем
SELECT FORMAT(Created, 'dd.MM.yyyy HH:mm:ss') Создан, 
	DocNumber AS Номер, 
	CustomerName AS Заказчик,	
	ISNULL(EquipmentName, '') AS Основание, 
	ISNULL(PatientFullName, '') AS Пациент,	
	ProductName AS Изделие,
	PricePositionCode AS [Код цены],
	ISNULL(MaterialName, '') AS Материал, 
	ISNULL(Color, '') AS Цвет, 
	COUNT(ProductName) AS Количество,
	ISNULL(Modeller, '') Моделировщик,
	ISNULL(Technician, '') AS Техник,
	ISNULL(Operator, '') AS Оператор,
	ISNULL(ImplantSystem, '') AS [Система имплантов], 
	ISNULL(SUM(Cashless), 0) AS Безнал, 
	ISNULL(SUM(Cash), 0) AS Нал,
	ISNULL(ResponsiblePerson, '') AS [Доктор/техник]

FROM ord
GROUP BY Created, DocNumber, CustomerName, EquipmentName, PatientFullName, ProductName, PricePositionCode,  MaterialName, Modeller, Technician, Operator, Color, ImplantSystem, ResponsiblePerson
)