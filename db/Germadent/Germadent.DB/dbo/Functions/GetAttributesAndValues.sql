-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 06.04.2020
-- Description:	Возвращает полный набор качественных атрибутов и их значений
-- =============================================
CREATE FUNCTION [dbo].[GetAttributesAndValues]
(	
	@attributeId int = NULL,
	@attrValueId int = NULL,
	@attributeKeyName varchar(50) = NULL
)
RETURNS TABLE 
AS
RETURN 
(
	
	SELECT a.AttributeID, a.AttributeKeyName, a.AttributeName, v.AttrValueID, v.AttributeValue
	FROM Attributes a INNER JOIN AttributesValues v ON a.AttributeID = v.AttributeID
	WHERE a.AttributeID = ISNULL(@attributeId, a.AttributeID)
		AND v.AttrValueID = ISNULL(@attrValueId, v.AttrValueID)
		AND a.AttributeKeyName LIKE '%'+ ISNULL(@attributeKeyName, '') + '%'
)