-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 22.02.2020
-- Description:	Закрытие заказ-наряда
-- =============================================
CREATE PROCEDURE [dbo].[CloseWorkOrder] 
	
	@workOrderId int	

AS
BEGIN

	SET NOCOUNT ON;

	-- Если заказ-наряд уже закрыт - никаких дальнейших действий
	IF((SELECT Status FROM WorkOrder WHERE WorkOrderID = @workOrderId) = 9)
		BEGIN
			RETURN
		END
		
	UPDATE WorkOrder
	SET Status = 9,
		Closed = GETDATE()
	WHERE WorkOrderID = @workOrderId

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CloseWorkOrder] TO [gdl_user]
    AS [dbo];

