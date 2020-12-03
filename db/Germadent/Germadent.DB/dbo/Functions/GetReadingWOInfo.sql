-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 03.12.2020
-- Description:	Возвращает пользователя, открывшего заказ-наряд, и время
-- =============================================
CREATE FUNCTION [dbo].[GetReadingWOInfo] 
(	
	@workOrderId int
)
RETURNS TABLE 
AS
RETURN 
(
	
	SELECT wo.DocNumber, CONCAT(u.FamilyName, ' ', LEFT(u.FirstName, 1), '.', LEFT(u.Patronymic, 1), '.') AS UserName, CONVERT(nvarchar, wo.ReadingDateTime, 113) AS ReadingDateTime
	FROM Users u INNER JOIN WorkOrder wo ON u.UserID = wo.ReaderUserID
	WHERE wo.WorkOrderID = @workOrderId

)