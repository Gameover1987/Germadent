-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 25.01.2020
-- Description:	Возвращает список изделий
-- =============================================
CREATE FUNCTION [dbo].[GetProducts]
(	
	
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT ProductID, ProductName
	FROM dbo.Products
)