-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 22.02.2020
-- Description:	Изменение статуса заказ-наряда
-- =============================================
CREATE PROCEDURE [dbo].[ChangeStatusWorkOrder] 
	
	@workOrderId int	,
	@status int,
	@dateTimeChange datetime output

AS
BEGIN

	SET NOCOUNT ON;

	-- Если заказ-наряд уже закрыт - никаких дальнейших действий
	IF((SELECT Status FROM dbo.WorkOrder WHERE WorkOrderID = @workOrderId) = 9)
		BEGIN
			RETURN
		END
		
	IF @status = 9 BEGIN 
		UPDATE dbo.WorkOrder
		SET Status = 9,
			Closed = GETDATE()
		WHERE WorkOrderID = @workOrderId
		END
	ELSE BEGIN
		UPDATE dbo.WorkOrder
		SET Status = @status
		WHERE WorkOrderID = @workOrderId
		END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ChangeStatusWorkOrder] TO [gdl_user]
    AS [dbo];

