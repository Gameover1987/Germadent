-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 27.05.2021
-- Description:	Возвращает сумму заработка сотрудника за заданный период в разрезе заказ-нарядов
-- =============================================
CREATE FUNCTION [dbo].[GetReportSalary]
(		
	@userId int, 
	--@dateCompletedFrom datetime,
	--@dateCompletedTo datetime,
	@dateStatusFrom datetime,
	@dateStatusTo datetime
)
RETURNS TABLE 
AS
RETURN 
(
	WITH currentStatus AS
	(SELECT WorkOrderID, Status, StatusChangeDateTime
		FROM dbo.StatusList stl
		WHERE stl.StatusChangeDateTime = (SELECT MAX(StatusChangeDateTime) 
											FROM dbo.StatusList stls  
											WHERE stl.WorkOrderID = stls.WorkOrderID 
											GROUP BY WorkOrderID)
	),

	matEnum AS
	(SELECT WorkOrderID, MaterialID, ProductID
		FROM dbo.ToothCard
		GROUP BY WorkOrderID, MaterialID, ProductID
	)
	
	SELECT u.UserID
	, CONCAT(u.FamilyName,' ', LEFT(u.FirstName, 1), '.', LEFT(u.Patronymic, 1), '.') AS UserFullName
	, wo.WorkOrderID
	, wo.DocNumber
	, c.CustomerName
	, wo.PatientFullName
	, p.ProductID
	, p.ProductName
	, m.MaterialID
	, m.MaterialName
	, teop.TechnologyOperationID
	, teop.TechnologyOperationUserCode
	, teop.TechnologyOperationName
	, wl.WorkID
	, wl.Rate
	, wl.Quantity
	, wo.UrgencyRatio
	, wl.OperationCost
	, FORMAT(currentStatus.StatusChangeDateTime, 'dd.MM.yyyy HH:mm:ss') AS StatusChangeDateTime
	, FORMAT(wl.WorkStarted, 'dd.MM.yyyy HH:mm:ss') AS WorkStarted
	, FORMAT(wl.WorkCompleted, 'dd.MM.yyyy HH:mm:ss') AS WorkCompleted

	FROM WorkOrder wo 
		INNER JOIN dbo.WorkList wl ON wo.WorkOrderID = wl.WorkOrderID
		INNER JOIN dbo.Users u ON wl.EmployeeIDStarted = u.UserID
		INNER JOIN dbo.Customers c ON wo.CustomerID = c.CustomerID
		INNER JOIN dbo.TechnologyOperations teop ON wl.TechnologyOperationID = teop.TechnologyOperationID
		INNER JOIN currentStatus ON wo.WorkOrderID = currentStatus.WorkOrderID
		LEFT JOIN Products p ON wl.ProductID = p.ProductID
		LEFT JOIN matEnum ON wl.WorkOrderID = matEnum.WorkOrderID AND wl.ProductID = matEnum.ProductID
		LEFT JOIN dbo.Materials m ON matEnum.MaterialID = m.MaterialID
	WHERE wl.EmployeeIDStarted = ISNULL(@userId, wl.EmployeeIDStarted)
		AND currentStatus.Status = 100
		AND currentStatus.StatusChangeDateTime BETWEEN ISNULL(@dateStatusFrom, '17530101') AND ISNULL(@dateStatusTo, '99991231')
)