-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 17.12.2019
-- Description:	Удаление заказ-наряда ЗТЛ
-- =============================================
CREATE PROCEDURE [dbo].[DeleteWorkOrderDL] 

	@workOrderId int,
	@countRowsDeleted int output

AS
BEGIN
	DECLARE
		@woStatus int

	SET @woStatus = (SELECT Status FROM WorkOrder WHERE WorkOrderID = @workOrderId)

	IF @woStatus = 0
			BEGIN
				DELETE
				FROM WorkOrderDL
				WHERE WorkOrderDLID = @workOrderId

				DELETE 
				FROM WorkOrder
				WHERE WorkOrderID = @workOrderId
			END
		ELSE IF @woStatus = 1
			UPDATE WorkOrder 
				SET Status = -1
				WHERE WorkOrderID = @workOrderId
	

	SELECT @countRowsDeleted = @@ROWCOUNT

END