
-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 18.10.2020
-- Description:	Возвращает список ролей с правами
-- =============================================
CREATE FUNCTION [dbo].[umc_GetRolesWithRights] 
(	
	
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT rl.RoleID, rl.RoleName, rg.RightID, rg.RightName, rg.RightDescription, rg.IsObsolete, a.ApplicationID, a.ApplicationName, a.ApplicationDescription
	FROM dbo.Roles rl
		INNER JOIN dbo.RolesAndRights rr ON rl.RoleID = rr.RoleID
		INNER JOIN dbo.Rights rg ON rg.RightID = rr.RightID
		INNER JOIN dbo.Applications a ON rg.ApplicationID = a.ApplicationID
)