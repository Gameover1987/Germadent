-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 28.05.2020
-- Description:	Возвращает все значения атрибутов
-- =============================================
CREATE FUNCTION [dbo].[GetAttrValues] 
(	
	
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT *
	FROM AttrValues
)