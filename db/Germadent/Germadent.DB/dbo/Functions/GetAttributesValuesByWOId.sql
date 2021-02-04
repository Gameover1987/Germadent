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
	
	SELECT s.WorkOrderID, s.ToothNumber, a.AttributeID, a.AttributeKeyName, a.AttributeName, v.AttributeValueID, v.AttributeValue
	FROM dbo.Attributes a 
		INNER JOIN dbo.AttrValues v ON a.AttributeID = v.AttributeID
		INNER JOIN dbo.AttributesSet s ON v.AttributeValueID = s.AttributeValueID
	WHERE s.WorkOrderID = @worklOrderId
	GROUP BY s.WorkOrderID, s.ToothNumber, a.AttributeID, a.AttributeKeyName, a.AttributeName, v.AttributeValueID, v.AttributeValue

)