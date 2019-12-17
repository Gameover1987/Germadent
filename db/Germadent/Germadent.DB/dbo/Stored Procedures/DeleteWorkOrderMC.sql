-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 17.12.2019
-- Description:	Удаление заказ-наряда ФЦ
-- =============================================
CREATE PROCEDURE [dbo].[DeleteWorkOrderMC]

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
				FROM WorkOrderMC
				WHERE WorkOrderMCID = @workOrderId

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