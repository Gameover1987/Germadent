-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 16.05.2021
-- Description:	Возвращает коэффициент срочности для заказ-наряда
-- =============================================
CREATE FUNCTION GetUrgencyRatioForWO
(
	@workOrderId int
)
RETURNS float
AS
BEGIN
	
	DECLARE @urgencyRatio float

	SELECT @urgencyRatio = UrgencyRatio FROM WorkOrder WHERE WorkOrderID = @workOrderId

	RETURN @urgencyRatio

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUrgencyRatioForWO] TO [gdl_user]
    AS [dbo];

