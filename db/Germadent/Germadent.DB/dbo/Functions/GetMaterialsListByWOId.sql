
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
	FROM dbo.ToothCard tc 
		INNER JOIN dbo.WorkOrder wo ON tc.WorkOrderID = wo.WorkOrderID
		INNER JOIN dbo.Materials m ON tc.MaterialID = m.MaterialID
	WHERE wo.WorkOrderID = @workOrderID
)