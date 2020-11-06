-- =============================================
-- Author:		 Алексей Колосенок
-- Create date:  23.11.2019
-- Editing date: 17.09.2020
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
	SELECT wo.WorkOrderID, tc.ToothNumber, m.MaterialID, m.MaterialName, c.ConditionID, c.ConditionName, p.ProstheticsID, p.ProstheticsName, tc.Price, tc.HasBridge, pg.PriceGroupID, pg.PriceGroupName, pp.PricePositionID, pp.PricePositionCode, pp.PricePositionName
	FROM ToothCard tc INNER JOIN WorkOrder wo ON tc.WorkOrderID = wo.WorkOrderID
		INNER JOIN ConditionsOfProsthetics c ON tc.ConditionID = c.ConditionID
		INNER JOIN PricePositions pp ON tc.PricePositionID = pp.PricePositionID
		INNER JOIN PriceGroups pg ON pp.PriceGroupID = pg.PriceGroupID
		LEFT JOIN TypesOfProsthetics p ON tc.ProstheticsID = p.ProstheticsID
		LEFT JOIN Materials m ON tc.MaterialID = m.MaterialID
	
				
	WHERE wo.WorkOrderID = @workOrderID
)