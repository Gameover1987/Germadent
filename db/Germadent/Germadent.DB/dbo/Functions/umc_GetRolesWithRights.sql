
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
	SELECT rl.RoleID, rl.RoleName, rg.RightID, rg.RightName, rg.RightDescription, a.ApplicationID, a.ApplicationName, a.ApplicationDescription
	FROM Roles rl
		INNER JOIN RolesAndRights rr ON rl.RoleID = rr.RoleID
		INNER JOIN Rights rg ON rg.RightID = rr.RightID
		INNER JOIN Applications a ON rg.ApplicationID = a.ApplicationID
)