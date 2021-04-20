-- =============================================
-- Author:		Алескей Колосенок
-- Create date: 20.04.2021
-- Description:	Возвращает список работ по заказ-наряду
-- =============================================
CREATE FUNCTION GetWorkListByWOId
(	
	@workOrderId int
)

RETURNS TABLE 
AS
RETURN 
(
	SELECT wl.WorkOrderID, 
			p.ProductID, 
			p.ProductName, 
			teo.TechnologyOperationID, 
			teo.TechnologyOperationUserCode, 
			teo.TechnologyOperationName, 
			u.UserID, 
			CONCAT(u.FamilyName,' ', LEFT(u.FirstName, 1), '.', LEFT(u.Patronymic, 1), '.') AS UserFullName, 
			wl.OperationCost, 
			wl.Started, 
			wl.Ended

	FROM dbo.WorkList wl
		INNER JOIN dbo.TechnologyOperations teo ON wl.TechnologyOperationID = teo.TechnologyOperationID
		INNER JOIN dbo.Users u ON wl.EmployeeID = u.UserID
		INNER JOIN dbo.Products p ON wl.ProductID = p.ProductID

	WHERE wl.WorkOrderID = @workOrderId
)