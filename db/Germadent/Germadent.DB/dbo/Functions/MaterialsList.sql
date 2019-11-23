-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 21.11.2019
-- Description:	Справочник материалов для зубной карты заказ-наряда
-- =============================================
CREATE FUNCTION [dbo].[MaterialsList] 
(	
	
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT MaterialID, MaterialName
	FROM Materials
	WHERE FlagUsed = 1
)
