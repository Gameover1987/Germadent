-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 20.04.2021
-- Description:	Возвращает список статусов для заказ-наряда
-- =============================================
CREATE FUNCTION [dbo].[GetStatusListForWO] 
(	
	@workOrderId int
)

RETURNS TABLE 
AS
RETURN 
(
	SELECT sl.WorkOrderID
	, se.StatusName
	, sl.StatusChangeDateTime
	, sl.UserID, CONCAT(u.FamilyName,' ', LEFT(u.FirstName, 1), '.', LEFT(u.Patronymic, 1), '.') AS UserFullName
	, sl.Remark

	FROM dbo.StatusList sl 
		INNER JOIN dbo.StatusEnumeration se ON sl.Status = se.Status
		INNER JOIN dbo.Users u ON sl.UserID = u.UserID

	WHERE sl.WorkOrderID = @workOrderId
)