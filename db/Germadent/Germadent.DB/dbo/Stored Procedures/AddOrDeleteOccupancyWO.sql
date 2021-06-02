-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 09.02.2021
-- Description:	Установка или снятие признака занятости заказ-наряда
-- =============================================
CREATE PROCEDURE [dbo].[AddOrDeleteOccupancyWO] 
	
	@workOrderID int, 
	@userID int = NULL
AS
BEGIN
	
	SET NOCOUNT ON;
	
	DECLARE
	@currentStatusWO int

	-- Определяем текущий статус заказ-наряда
	SELECT @currentStatusWO = Status
	FROM dbo.StatusList
	WHERE WorkOrderID = @workOrderId AND StatusChangeDateTime = (SELECT MAX(StatusChangeDateTime)
																	FROM dbo.StatusList
																	WHERE WorkOrderID = @workOrderId)

	-- Если заказ-наряд уже закрыт - никаких дальнейших действий
	IF @currentStatusWO = 100
		RETURN

	IF @userID IS NULL
		BEGIN
			DELETE 
			FROM OccupancyWO
			WHERE WorkOrderID = @workOrderID
			RETURN
		END
	ELSE BEGIN 
			INSERT INTO OccupancyWO
			(WorkOrderID, OccupancyDateTime, UserID)
			VALUES
			(@workOrderID, GETDATE(), @userID)
		END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddOrDeleteOccupancyWO] TO [gdl_user]
    AS [dbo];

