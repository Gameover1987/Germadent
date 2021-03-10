-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 18.10.2020
-- Description:	Возвращает список ролей
-- =============================================
CREATE FUNCTION [dbo].[umc_GetRights] 
(	
	
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT rg.RightID, rg.RightName, rg.RightDescription, rg.IsObsolete, a.ApplicationID, a.ApplicationName, a.ApplicationDescription
	FROM dbo.Rights rg 
		INNER JOIN dbo.Applications a ON rg.ApplicationID = a.ApplicationID
)