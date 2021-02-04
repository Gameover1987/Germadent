-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 06.04.2020
-- Description:	Возвращает полный набор качественных атрибутов и их значений
-- =============================================
CREATE FUNCTION [dbo].[GetAttributesAndValues]
(	
	
)
RETURNS TABLE 
AS
RETURN 
(
	
	SELECT a.AttributeID, a.AttributeKeyName, a.AttributeName, a.IsObsolete, v.AttributeValueID, v.AttributeValue
	FROM dbo.Attributes a INNER JOIN dbo.AttrValues v ON a.AttributeID = v.AttributeID
	
)