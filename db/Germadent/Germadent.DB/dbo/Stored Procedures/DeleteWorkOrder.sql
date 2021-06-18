-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 17.12.2019
-- Description:	Удаление заказ-наряда
-- =============================================
CREATE PROCEDURE [dbo].[DeleteWorkOrder] 

	@workOrderId int,
	@userId int,	
	@countRowsDeleted int = 0 output

AS
BEGIN
	DECLARE
		@woStatus int,
		@statusChangeDateTime datetime

	SET @woStatus = (SELECT MAX(Status) FROM dbo.StatusList WHERE WorkOrderID = @workOrderId)
		
	IF @woStatus = 0
			BEGIN
				DELETE 
				FROM dbo.WorkOrder
				WHERE WorkOrderID = @workOrderId

				SELECT @countRowsDeleted = @@ROWCOUNT
			END

		ELSE IF @woStatus = 5 OR @woStatus = 100
			RETURN

			ELSE BEGIN
			EXEC dbo.ChangeStatusWorkOrder @workOrderId, 5, @userId, null, @statusChangeDateTime
			END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteWorkOrder] TO [gdl_user]
    AS [dbo];

