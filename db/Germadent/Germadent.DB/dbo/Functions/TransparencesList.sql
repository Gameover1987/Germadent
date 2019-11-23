-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 23.11.2019
-- Description:	Справочник прозрачностей зубов
-- =============================================
CREATE FUNCTION TransparencesList 
(	
	
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT TransparenceID, TransparenceName
	FROM Transparences
)
