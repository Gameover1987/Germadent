-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 23.11.2019
-- Description:	Зубная карта из заказ-наряда по его ID
-- =============================================
CREATE FUNCTION [dbo].[GetToothCardByWOId] 
(	
	
	@WorkOrderID int
	
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT wo.WorkOrderID, tc.ToothNumber, c.ConditionName, p.ProstheticsName, m.MaterialName, tc.FlagBridge 
	FROM ToothCard tc INNER JOIN WorkOrder wo ON tc.WorkOrderID = wo.WorkOrderID
		INNER JOIN Materials m ON tc.MaterialID = m.MaterialID
		INNER JOIN TypesOfProsthetics p ON tc.ProstheticsID = p.ProstheticsID
		INNER JOIN ConditionsOfProsthetics c ON tc.ConditionID = c.ConditionID
	WHERE wo.WorkOrderID = @WorkOrderID
)