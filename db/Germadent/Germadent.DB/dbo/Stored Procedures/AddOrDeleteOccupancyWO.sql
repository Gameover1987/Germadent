-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 09.02.2021
-- Description:	Установка или уборка признака занятости заказ-наряда
-- =============================================
CREATE PROCEDURE [dbo].[AddOrDeleteOccupancyWO] 
	
	@workOrderID int, 
	@userID int = NULL
AS
BEGIN
	
	SET NOCOUNT ON;

	-- Никаких изменений, если заказ-наряд закрыт
	IF((SELECT Status FROM dbo.WorkOrder WHERE WorkOrderID = @workOrderID) = 9)
		BEGIN
			RETURN
		END

	IF @userID IS NULL
		BEGIN
			DELETE 
			FROM BusyDocs
			WHERE WorkOrderID = @workOrderID
			RETURN
		END
	ELSE BEGIN 
			INSERT INTO BusyDocs
			(WorkOrderID, OccupancyDateTime, UserID)
			VALUES
			(@workOrderID, GETDATE(), @userID)
		END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddOrDeleteOccupancyWO] TO [gdl_user]
    AS [dbo];

