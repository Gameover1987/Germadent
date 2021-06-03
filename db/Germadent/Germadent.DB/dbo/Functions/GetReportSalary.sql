-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 27.05.2021
-- Description:	Возвращает сумму заработка сотрудника за заданный период в разрезе заказ-нарядов
-- =============================================
CREATE FUNCTION [dbo].[GetReportSalary]
(		
	@userId int, 
	@dateStartedFrom datetime,
	@dateStartedTo datetime,
	@dateCompletedFrom datetime,
	@dateCompletedTo datetime
)
RETURNS TABLE 
AS
RETURN 
(
	WITH currentStatus (WorkOrderID, Status, StatusChangeDateTime) AS
	(SELECT WorkOrderID, Status, StatusChangeDateTime
		FROM dbo.StatusList stl
		WHERE stl.StatusChangeDateTime = (SELECT MAX(StatusChangeDateTime) 
											FROM dbo.StatusList stls  
											WHERE stl.WorkOrderID = stls.WorkOrderID 
											GROUP BY WorkOrderID)
	)

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
	, wl.Rate
	, wl.Quantity
	, wo.UrgencyRatio
	, wl.OperationCost
	, wl.WorkStarted
	, wl.WorkCompleted

	FROM WorkOrder wo 
		INNER JOIN dbo.WorkList wl ON wo.WorkOrderID = wl.WorkOrderID
		INNER JOIN dbo.Users u ON wl.EmployeeIDStarted = u.UserID
		INNER JOIN dbo.Customers c ON wo.CustomerID = c.CustomerID
		INNER JOIN dbo.TechnologyOperations teop ON wl.TechnologyOperationID = teop.TechnologyOperationID
		INNER JOIN currentStatus ON wo.WorkOrderID = currentStatus.WorkOrderID
		LEFT JOIN Products p ON wl.ProductID = p.ProductID
	WHERE wl.EmployeeIDStarted = ISNULL(@userId, wl.EmployeeIDStarted)
		AND ((wo.BranchTypeID = 2 AND currentStatus.Status = 100) OR (wo.BranchTypeID = 1 AND currentStatus.Status > 80))
		AND wl.WorkStarted BETWEEN ISNULL(@dateStartedFrom, '17530101') AND ISNULL(@dateStartedTo, '99991231')
		AND wl.WorkCompleted BETWEEN ISNULL(@dateCompletedFrom, '17530101') AND ISNULL(@dateCompletedTo, '99991231')
)