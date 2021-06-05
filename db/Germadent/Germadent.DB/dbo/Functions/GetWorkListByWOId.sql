-- =============================================
-- Author:		Алескей Колосенок
-- Create date: 20.04.2021
-- Description:	Возвращает список работ по заказ-наряду
-- =============================================
CREATE FUNCTION [dbo].[GetWorkListByWOId]
(	
	@workOrderId int = NULL,
	@userId int = NULL
)

RETURNS TABLE 
AS
RETURN 
(
	WITH emploId (EmployeePositionID) AS
		(SELECT EmployeePositionID 
			FROM dbo.EmployeePositionsCombination 
			WHERE EmployeeID = @userId)
	
	SELECT 
			wl.WorkID,
			wl.WorkOrderID, 
			p.ProductID, 
			p.ProductName, 
			teo.TechnologyOperationID, 
			teo.TechnologyOperationUserCode, 
			teo.TechnologyOperationName,
			teo.EmployeePositionID,
			wl.EmployeeIDStarted AS UserIdStarted, 
			CONCAT(u.FamilyName,' ', LEFT(u.FirstName, 1), '.', LEFT(u.Patronymic, 1), '.') AS UserFullNameStarted,
			wl.Rate,
			wl.Quantity,
			wo.UrgencyRatio,
			wl.OperationCost, 
			wl.WorkStarted, 
			wl.WorkCompleted,
			wl.EmployeeIDCompleted AS UserIdCompleted,
			CONCAT(uc.FamilyName,' ', LEFT(uc.FirstName, 1), '.', LEFT(uc.Patronymic, 1), '.') AS UserFullNameCompleted,
			wl.Comment

	FROM dbo.WorkList wl
		INNER JOIN dbo.TechnologyOperations teo ON wl.TechnologyOperationID = teo.TechnologyOperationID
		INNER JOIN dbo.Users u ON wl.EmployeeIDStarted = u.UserID
		LEFT JOIN dbo.Products p ON wl.ProductID = p.ProductID
		LEFT JOIN dbo.Users uc ON wl.EmployeeIDCompleted = uc.UserID,
		dbo.WorkOrder wo

	WHERE wl.WorkOrderID = wo.WorkOrderID
		AND wl.WorkOrderID = ISNULL(@workOrderId, wl.WorkOrderID)
		AND (wl.EmployeeIDStarted = ISNULL(@userId, wl.EmployeeIDStarted) OR teo.EmployeePositionID = (SELECT EmployeePositionID FROM emploId WHERE EmployeePositionID = 4))
)