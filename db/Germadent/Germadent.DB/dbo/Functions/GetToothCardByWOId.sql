-- =============================================
-- Author:		 Алексей Колосенок
-- Create date:  23.11.2019
-- Editing date: 28.06.2020
-- Description:	 Зубная карта из заказ-наряда по его ID
-- =============================================
CREATE FUNCTION [dbo].[GetToothCardByWOId] 
(
	@workOrderID int	
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT wo.WorkOrderID, tc.ToothNumber, m.MaterialName, s.SeviceName, tc.Price, cc.ConstructionColorName, c.ConditionName, p.ProstheticsName, t.TransparencyName, tc.FlagBridge, pg.PriceGroupCode

	FROM ToothCard tc INNER JOIN WorkOrder wo ON tc.WorkOrderID = wo.WorkOrderID
		INNER JOIN Servicess s ON tc.ServiceID = s.ServiceID
		INNER JOIN Materials m ON s.MaterialID = m.MaterialID
		INNER JOIN PriceGroups pg ON pg.PriceGroupID = s.PriceGroupID
		INNER JOIN ConstructionColors cc ON tc.ContructionColorID = cc.ConstructionColorID
		INNER JOIN ConditionsOfProsthetics c ON tc.ConditionID = c.ConditionID
		INNER JOIN TypesOfProsthetics p ON tc.ProstheticsID = p.ProstheticsID
		INNER JOIN Transparences t ON tc.TrasparencyID = t.TransparencyID
				
	WHERE wo.WorkOrderID = @workOrderID
)