-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 23.11.2019
-- Description:	Справочник прозрачностей зубов
-- =============================================
CREATE FUNCTION [GetTransparencesList] 
(	
	
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT TransparenceID, TransparenceName
	FROM Transparences
)