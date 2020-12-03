-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 02.12.2020
-- Description:	Увеломление для клиента об открытии заказ-наряда
-- =============================================
CREATE PROCEDURE [dbo].[UserReadingWO] 
	
	@workOrderID int,
	@readerUserId int = NULL
	
AS
BEGIN
	
	SET NOCOUNT ON;

	DECLARE @readingDateTime datetime

    	-- Никаких изменений, если заказ-наряд закрыт
	IF((SELECT Status FROM WorkOrder WHERE WorkOrderID = @workOrderID) = 9)
		BEGIN
			RETURN
		END

	IF @readerUserId IS NULL 
		SET @readingDateTime = NULL
	ELSE SET @readingDateTime = GETDATE()

	UPDATE WorkOrder
	SET ReaderUserID = @readerUserId,
		ReadingDateTime = @readingDateTime

	WHERE WorkOrderID = @workOrderID

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UserReadingWO] TO [gdl_user]
    AS [dbo];

