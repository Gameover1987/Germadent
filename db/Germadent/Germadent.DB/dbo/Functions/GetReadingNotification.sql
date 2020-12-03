-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 03.12.2020
-- Description:	Возвращает строку с ФИО пользователя и датой/временем открытия заказ-наряда на чтение
-- =============================================
CREATE FUNCTION [dbo].[GetReadingNotification]
(
	@workOrderId int
)
RETURNS nvarchar(255)
AS
BEGIN
	
	DECLARE @result nvarchar(255)

	SELECT @result = CONCAT('Заказ-наряд ', wo.DocNumber, ' пока доступен только для чтения. /r/nПользователь ', u.FamilyName, ' ', LEFT(u.FirstName, 1), '.', LEFT(u.Patronymic, 1), '. заблокировал его ', CONVERT(nvarchar, wo.ReadingDateTime, 113))
	FROM Users u INNER JOIN WorkOrder wo ON u.UserID = wo.ReaderUserID
	WHERE wo.WorkOrderID = @workOrderId

	RETURN @result

END