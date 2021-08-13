-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 18.07.2021
-- Description:	Возвращает сумму заработка сотрудника, посчитанную 2 способами, за заданный период в разрезе заказ-нарядов
-- =============================================
CREATE FUNCTION [dbo].[GetReportVariousSalary]
(
	@userId int,
	@dateStatusFrom datetime,
	@dateStatusTo datetime
)
RETURNS 
@result TABLE 
(	UserID int
	, UserFullName nvarchar(150)
	, WorkOrderID int
	, DocNumber nvarchar(10)
	, CustomerName nvarchar(70)
	, PatientFullName nvarchar(150)
	, ProductID int
	, ProductName nvarchar(255)
	, TechnologyOperationID int
	, TechnologyOperationUserCode nvarchar(20)
	, TechnologyOperationName nvarchar(250)
	, WorkID int
	, Rate money
	, Quantity int
	, UrgencyRatio float
	, OperationCost money
	, StatusChangeDateTime datetime
	, WorkStarted datetime
	, WorkCompleted datetime
)
AS
BEGIN

	DECLARE @userSpecialId int = 12

	DECLARE @currentStatus TABLE
	(WorkOrderID int
	, Status int
	, StatusChangeDateTime datetime)
	
	DECLARE @salesOfProducts TABLE
	(WorkOrderID int
	, PricePositionID int
	, PricePositionCode nvarchar(10)
	, ProductID int
	, Quantity int
	, OperationCost money)

	DECLARE @specialSalary TABLE
	(UserID int
	, UserFullName nvarchar(150)
	, WorkOrderID int
	, DocNumber nvarchar(10)
	, CustomerName nvarchar(70)
	, PatientFullName nvarchar(150)
	, ProductID int
	, ProductName nvarchar(255)
	, TechnologyOperationID int
	, TechnologyOperationUserCode nvarchar(20)
	, TechnologyOperationName nvarchar(250)
	, WorkID int
	, Rate money
	, Quantity int
	, UrgencyRatio float
	, OperationCost money
	, StatusChangeDateTime datetime
	, WorkStarted datetime
	, WorkCompleted datetime);

	INSERT @currentStatus 
	SELECT WorkOrderID, Status, StatusChangeDateTime
		FROM dbo.StatusList stl
		WHERE stl.StatusChangeDateTime = (SELECT MAX(StatusChangeDateTime) 
											FROM dbo.StatusList stls  
											WHERE stl.WorkOrderID = stls.WorkOrderID 
											GROUP BY WorkOrderID)

IF @userId = @userSpecialId OR @userId IS NULL BEGIN

	INSERT @salesOfProducts
	SELECT tc.WorkOrderID, tc.PricePositionID, pp.PricePositionCode, tc.ProductID, COUNT(tc.ProductID), SUM(Price) * 0.3 + 220 * COUNT(tc.ProductID)
	FROM ToothCard tc
		INNER JOIN PricePositions pp ON tc.PricePositionID = pp.PricePositionID
	WHERE tc.WorkOrderID IN (SELECT WorkOrderID 
								FROM dbo.WorkList
								WHERE EmployeeIDStarted = @userSpecialId)
	GROUP BY tc.WorkOrderID, tc.PricePositionID, pp.PricePositionCode, tc.ProductID

	INSERT @specialSalary
	SELECT wl.EmployeeIDStarted AS UserID
	, CONCAT(u.FamilyName,' ', LEFT(u.FirstName, 1), '.', LEFT(u.Patronymic, 1), '.') AS UserFullName
	, wl.WorkOrderID
	, wo.DocNumber
	, c.CustomerName
	, wo.PatientFullName
	, sop.ProductID
	, p.ProductName
	, teop.TechnologyOperationID
	, teop.TechnologyOperationUserCode
	, teop.TechnologyOperationName
	, wl.WorkID
	, ROUND((sop.OperationCost)/ wo.UrgencyRatio / sop.Quantity, 2) AS Rate
	, sop.Quantity
	, wo.UrgencyRatio
	, sop.OperationCost AS OperationCost
	, cs.StatusChangeDateTime
	, wl.WorkStarted
	, wl.WorkCompleted
FROM @salesOfProducts sop
	INNER JOIN dbo.WorkList wl on sop.WorkOrderID = wl.WorkOrderID AND sop.ProductID = wl.ProductID
	INNER JOIN dbo.TechnologyOperations teop ON wl.TechnologyOperationID = teop.TechnologyOperationID
	INNER JOIN dbo.WorkOrder wo ON sop.WorkOrderID = wo.WorkOrderID
	INNER JOIN dbo.Users u ON wl.EmployeeIDStarted = u.UserID
	INNER JOIN dbo.Customers c ON wo.CustomerID = c.CustomerID
	INNER JOIN dbo.Products p ON wl.ProductID = p.ProductID
	INNER JOIN @currentStatus cs ON wo.WorkOrderID = cs.WorkOrderID
WHERE LEFT(sop.PricePositionCode, 3) = teop.TechnologyOperationUserCode
	AND cs.Status = 100
	AND cs.StatusChangeDateTime BETWEEN ISNULL(@dateStatusFrom, '17530101') AND ISNULL(@dateStatusTo, '99991231')
END

INSERT @result
	SELECT u.UserID
	, CONCAT(u.FamilyName,' ', LEFT(u.FirstName, 1), '.', LEFT(u.Patronymic, 1), '.') AS UserFullName
	, wo.WorkOrderID
	, wo.DocNumber
	, c.CustomerName
	, wo.PatientFullName
	, p.ProductID
	, p.ProductName
	, teop.TechnologyOperationID
	, teop.TechnologyOperationUserCode
	, teop.TechnologyOperationName
	, wl.WorkID
	, CASE WHEN wl.EmployeeIDStarted = @userSpecialId AND teop.TechnologyOperationID = 1 THEN 220 ELSE wl.Rate END Rate
	, wl.Quantity
	, wo.UrgencyRatio
	, CASE WHEN wl.EmployeeIDStarted = @userSpecialId AND teop.TechnologyOperationID = 1 THEN 220 * wl.Quantity * wo.UrgencyRatio ELSE wl.OperationCost END OperationCost
	, cs.StatusChangeDateTime
	, wl.WorkStarted
	, wl.WorkCompleted

	FROM WorkOrder wo 
		INNER JOIN dbo.WorkList wl ON wo.WorkOrderID = wl.WorkOrderID
		INNER JOIN dbo.Users u ON wl.EmployeeIDStarted = u.UserID
		INNER JOIN dbo.Customers c ON wo.CustomerID = c.CustomerID
		INNER JOIN dbo.TechnologyOperations teop ON wl.TechnologyOperationID = teop.TechnologyOperationID
		INNER JOIN @currentStatus cs ON wo.WorkOrderID = cs.WorkOrderID
		INNER JOIN dbo.Products p ON wl.ProductID = p.ProductID
	WHERE wl.EmployeeIDStarted = ISNULL(@userId, wl.EmployeeIDStarted)
		AND cs.Status = 100
		AND cs.StatusChangeDateTime BETWEEN ISNULL(@dateStatusFrom, '17530101') AND ISNULL(@dateStatusTo, '99991231')
		AND (wl.EmployeeIDStarted != @userSpecialId OR teop.TechnologyOperationID = 1)

	UNION ALL
	SELECT *
	FROM @specialSalary

	RETURN 
END