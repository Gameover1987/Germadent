-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 22.02.2020
-- Description:	Изменение статуса заказ-наряда
-- =============================================
CREATE PROCEDURE [dbo].[ChangeStatusWorkOrder] 
	
	@workOrderId int,
	@status int,
	@userId int,
	@remark nvarchar(100),
	@statusChangeDateTime datetime output

AS
BEGIN

	SET NOCOUNT, XACT_ABORT ON;

	-- Если заказ-наряд уже закрыт - никаких дальнейших действий
	IF EXISTS (SELECT 1 FROM dbo.StatusList WHERE WorkOrderID = @workOrderId AND Status = 100)
		RETURN
		
	ELSE BEGIN

		BEGIN TRAN
		SET @statusChangeDateTime = GETDATE()

		INSERT INTO dbo.StatusList
		(WorkOrderID, Status, StatusChangeDateTime, UserID, Remark)
		VALUES
		(@workOrderId, @status, @statusChangeDateTime, @userId, @remark)
		COMMIT

	END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ChangeStatusWorkOrder] TO [gdl_user]
    AS [dbo];

