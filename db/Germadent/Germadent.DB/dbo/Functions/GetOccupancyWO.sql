-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 05.02.2021
-- Description:	Возвращает данные о том, кто и когда открыл заказ-наряд
-- =============================================
CREATE FUNCTION [dbo].[GetOccupancyWO] 
(	
	@workOrderId int = NULL 
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT o.WorkOrderID
			, wo.DocNumber
			, o.OccupancyDateTime
			, o.UserID
			, CONCAT(u.FamilyName,' ', LEFT(u.FirstName, 1), '.', LEFT(u.Patronymic, 1), '.') AS UserFullName
	FROM dbo.OccupancyWO o
		INNER JOIN dbo.WorkOrder wo ON o.WorkOrderID = wo.WorkOrderID
		INNER JOIN dbo.Users u ON o.UserID = u.UserID
	WHERE o.WorkOrderID = ISNULL(@workorderID, o.WorkOrderID)
)