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
	SELECT 
			wl.WorkOrderID, 
			p.ProductID, 
			p.ProductName, 
			teo.TechnologyOperationID, 
			teo.TechnologyOperationUserCode, 
			teo.TechnologyOperationName, 
			u.UserID, 
			CONCAT(u.FamilyName,' ', LEFT(u.FirstName, 1), '.', LEFT(u.Patronymic, 1), '.') AS EmployeeFullName,
			wl.Rate,
			wl.Quantity,
			wl.OperationCost, 
			wl.WorkStarted, 
			wl.WorkCompleted

	FROM dbo.WorkList wl
		INNER JOIN dbo.TechnologyOperations teo ON wl.TechnologyOperationID = teo.TechnologyOperationID
		INNER JOIN dbo.Users u ON wl.EmployeeID = u.UserID
		LEFT JOIN dbo.Products p ON wl.ProductID = p.ProductID

	WHERE wl.WorkOrderID = ISNULL(@workOrderId, wl.WorkOrderID)
		AND wl.EmployeeID = ISNULL(@userId, wl.EmployeeID)
)