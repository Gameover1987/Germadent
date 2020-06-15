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
	
	SELECT a.AttributeID, a.AttributeKeyName, a.AttributeName, a.FlagObsolete, v.AttrValueID, v.AttributeValue
	FROM Attributes a INNER JOIN AttrValues v ON a.AttributeID = v.AttributeID
	
)