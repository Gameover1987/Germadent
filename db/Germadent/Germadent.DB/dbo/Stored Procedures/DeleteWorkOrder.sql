﻿-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 17.12.2019
-- Description:	Удаление заказ-наряда
-- =============================================
CREATE PROCEDURE [dbo].[DeleteWorkOrder] 

	@workOrderId int,
	@userId int,
	@statusChangeDateTime datetime,
	@countRowsDeleted int = 0 output

AS
BEGIN
	DECLARE
		@woStatus int

	SET @woStatus = (SELECT MAX(Status) FROM dbo.StatusList WHERE WorkOrderID = @workOrderId)

	IF @woStatus = 0
			BEGIN
				DELETE 
				FROM dbo.WorkOrder
				WHERE WorkOrderID = @workOrderId

				SELECT @countRowsDeleted = @@ROWCOUNT
			END

		ELSE IF @woStatus = -1
			RETURN

			ELSE BEGIN
			SET @statusChangeDateTime = GETDATE()
			EXEC dbo.ChangeStatusWorkOrder @workOrderId, @userId, null, @statusChangeDateTime
			END	

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteWorkOrder] TO [gdl_user]
    AS [dbo];

