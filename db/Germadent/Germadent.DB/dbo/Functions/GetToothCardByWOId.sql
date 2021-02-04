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
	SELECT wo.WorkOrderID, tc.ToothNumber, m.MaterialID, m.MaterialName, c.ConditionID, c.ConditionName, p.ProductID, p.ProductName, tc.Price, tc.HasBridge, pg.PriceGroupID, pg.PriceGroupName, pp.PricePositionID, pp.PricePositionCode, pp.PricePositionName
	FROM dbo.ToothCard tc 
		INNER JOIN dbo.WorkOrder wo ON tc.WorkOrderID = wo.WorkOrderID
		INNER JOIN dbo.ConditionsOfProsthetics c ON tc.ConditionID = c.ConditionID
		INNER JOIN dbo.PricePositions pp ON tc.PricePositionID = pp.PricePositionID
		INNER JOIN dbo.PriceGroups pg ON pp.PriceGroupID = pg.PriceGroupID
		LEFT JOIN dbo.Products p ON tc.ProductID = p.ProductID
		LEFT JOIN dbo.Materials m ON tc.MaterialID = m.MaterialID
	
				
	WHERE wo.WorkOrderID = @workOrderID
)