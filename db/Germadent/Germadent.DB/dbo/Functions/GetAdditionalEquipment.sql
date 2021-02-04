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
	SELECT a.WorkOrderID, a.EquipmentID, e.EquipmentName, a.QuantityIn, a.QuantityOut
	FROM dbo.AdditionalEquipment a 
		INNER JOIN dbo.Equipments e ON a.EquipmentID = e.EquipmentID

	WHERE WorkOrderID = @workOrderId
)