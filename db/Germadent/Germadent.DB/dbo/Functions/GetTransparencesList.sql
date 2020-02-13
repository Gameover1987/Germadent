-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 23.11.2019
-- Description:	Возвращает список прозрачностей зубов
-- =============================================
CREATE FUNCTION [dbo].[GetTransparencesList] 
(	
	
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT TransparenceID, TransparenceName
	FROM Transparences
)