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
	SELECT wo.WorkOrderID, tc.ToothNumber, m.MaterialName, P.ProductName, tc.Price, tc.FlagBridge, pp.PricePositionCode, pp.PricePositionName
	FROM ToothCard tc INNER JOIN WorkOrder wo ON tc.WorkOrderID = wo.WorkOrderID
		INNER JOIN PricePositions pp ON tc.PricePositionID = pp.PricePositionID
		LEFT JOIN Product p ON tc.ProductID = p.ProductID
		LEFT JOIN Materials m ON tc.MaterialID = m.MaterialID
	
				
	WHERE wo.WorkOrderID = @workOrderID
)