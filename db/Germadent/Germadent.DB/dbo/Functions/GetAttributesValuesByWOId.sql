-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 14.04.2020
-- Description:	Возвращает набор качественных атрибутов и их значений для заказ-наряда или зубной карты
-- =============================================
CREATE FUNCTION [dbo].[GetAttributesValuesByWOId]
(	
	@worklOrderId int
)
RETURNS TABLE 
AS
RETURN 
(
	
	SELECT s.WorkOrderID, s.ToothNumber, a.AttributeID, a.AttributeKeyName, a.AttributeName, STRING_AGG(v.AttributeValueID, '; ') AS AttributeValueID, STRING_AGG(v.AttributeValue, '; ') AS AttributeValue
	FROM Attributes a 
		INNER JOIN AttrValues v ON a.AttributeID = v.AttributeID
		INNER JOIN AttributesSet s ON v.AttributeValueID = s.AttributeValueID
	WHERE s.WorkOrderID = @worklOrderId
	GROUP BY s.WorkOrderID, s.ToothNumber, a.AttributeID, a.AttributeKeyName, a.AttributeName, v.AttributeValueID, v.AttributeValue

)