-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 06.04.2020
-- Description:	Возвращает полный набор качественных атрибутов и их значений
-- =============================================
CREATE FUNCTION [dbo].[GetAttributesAndValues]
(	
	@attributeId int = NULL,
	@qValueId int = NULL,
	@attributeKeyName varchar(50) = NULL
)
RETURNS TABLE 
AS
RETURN 
(
	
	SELECT a.AttributeID, a.AttributeKeyName, a.AttributeName, v.QValueID, v.QualitativeValue
	FROM QualitativeAttributes a INNER JOIN QualitativeValues v ON a.AttributeID = v.AttributeID
	WHERE a.AttributeID = ISNULL(@attributeId, a.AttributeID)
		AND v.QValueID = ISNULL(@qValueId, v.QValueID)
		AND a.AttributeKeyName LIKE '%'+ ISNULL(@attributeKeyName, '') + '%'
)