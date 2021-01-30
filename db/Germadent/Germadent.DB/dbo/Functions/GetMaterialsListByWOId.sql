
-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 21.11.2019
-- Description:	Возвращает список материалов из заказ-наряда
-- =============================================
CREATE FUNCTION [dbo].[GetMaterialsListByWOId] 
(	
	@workOrderID int
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT DISTINCT m.MaterialName
	FROM ToothCard tc INNER JOIN WorkOrder wo ON tc.WorkOrderID = wo.WorkOrderID
		INNER JOIN Materials m ON tc.MaterialID = m.MaterialID
	WHERE wo.WorkOrderID = @workOrderID
)