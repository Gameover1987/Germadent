-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 16.02.2020
-- Description:	Возвращает набор идентификаторов для заказ-наряда и оснастки, а также количество оснастки 
-- =============================================
CREATE FUNCTION [dbo].[GetAdditionalEquipment]
(	
	@workOrderId int = NULL
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT *
	FROM AdditionalEquipment
	WHERE WorkOrderID = @workOrderId
)