-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 14.04.2020
-- Description:	Возвращает набор качественных атрибутов и их значений для заказ-наряда
-- =============================================
CREATE FUNCTION [dbo].[GetAttributesValuesByWOId]
(	
	@worklOrderId int
)
RETURNS TABLE 
AS
RETURN 
(
	
	SELECT s.WorkOrderID, a.AttributeID, a.AttributeKeyName, a.AttributeName, STRING_AGG(v.AttrValueID, '; ') AS AttrValueID, STRING_AGG(v.AttributeValue, '; ') AS AttributeValue
	FROM Attributes a INNER JOIN AttrValues v ON a.AttributeID = v.AttributeID
		INNER JOIN AttributesSet s ON v.AttrValueID = s.AttrValueID
	WHERE s.WorkOrderID = @worklOrderId
	GROUP BY s.WorkOrderID, a.AttributeID, a.AttributeKeyName, a.AttributeName, v.AttrValueID, v.AttributeValue

)