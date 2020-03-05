-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 17.12.2019
-- Description:	Удаление заказ-наряда
-- =============================================
CREATE PROCEDURE [dbo].[DeleteWorkOrder] 

	@workOrderId int
--	@countRowsDeleted int output

AS
BEGIN
	DECLARE
		@woStatus int

	SET @woStatus = (SELECT Status FROM WorkOrder WHERE WorkOrderID = @workOrderId)

	IF @woStatus = 0
			BEGIN
				DELETE 
				FROM WorkOrder
				WHERE WorkOrderID = @workOrderId
			END
		ELSE IF @woStatus = 1
			UPDATE WorkOrder 
				SET Status = -1
				WHERE WorkOrderID = @workOrderId
	
	--UPDATE StlAndPhotos
	--SET file_stream = CONVERT(varbinary(max), '0')
	--WHERE stream_id NOT IN (SELECT stream_id FROM LinksFileStreams)

	DELETE
	FROM StlAndPhotos 
	WHERE stream_id NOT IN (SELECT stream_id FROM LinksFileStreams)
		

--	SELECT @countRowsDeleted = @@ROWCOUNT

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteWorkOrder] TO [gdl_user]
    AS [dbo];

