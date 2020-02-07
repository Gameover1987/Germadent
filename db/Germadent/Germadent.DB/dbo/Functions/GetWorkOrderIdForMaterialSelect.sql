-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 07.02.2020
-- Description:	Возвращает список id заказ-нарядов, в зубных картах которых встречаются искомые материалы
-- =============================================
CREATE FUNCTION [dbo].[GetWorkOrderIdForMaterialSelect] 
(	
	@materialSet nvarchar(MAX)
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT tc.WorkOrderID
	FROM ToothCard tc inner join Materials m on tc.MaterialID = m.MaterialID
	WHERE m.MaterialName IN 
	(SELECT MaterialName 
		FROM OPENJSON(@materialSet) 
		WITH (materialName nvarchar(50)))
	GROUP BY tc.WorkOrderID, m.MaterialName
)