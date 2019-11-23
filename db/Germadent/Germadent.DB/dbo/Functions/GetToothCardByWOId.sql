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
	SELECT tc.ToothNumber, m.MaterialName, p.ProstheticsName
	FROM ToothCard tc INNER JOIN WorkOrder wo ON tc.WorkOrderID = wo.WorkOrderID
		INNER JOIN Materials m ON tc.MaterialID = m.MaterialID
		INNER JOIN TypesOfProsthetics p ON tc.ProstheticsID = p.ProstheticsID
	WHERE wo.WorkOrderID = @WorkOrderID
)