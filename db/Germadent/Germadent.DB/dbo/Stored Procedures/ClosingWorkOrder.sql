-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 22.02.2020
-- Description:	Закрытие заказ-наряда
-- =============================================
CREATE PROCEDURE [dbo].[ClosingWorkOrder] 
	
	@workOrderId int

AS
BEGIN
	
	SET NOCOUNT ON;

	UPDATE WorkOrder
	SET Status = 2,
		Closed = GETDATE()
	WHERE WorkOrderID = @workOrderId

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ClosingWorkOrder] TO [gdl_user]
    AS [dbo];

