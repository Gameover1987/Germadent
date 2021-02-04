-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 21.10.2020
-- Description:	Возвращает авторизованного пользователя
-- =============================================
CREATE FUNCTION [dbo].[umc_Authorization] 
(	
	
	@login nvarchar(100), 
	@password nvarchar(MAX)

)
RETURNS TABLE 
AS
RETURN 
(
	
	SELECT u.UserID, Login, FamilyName, FirstName, Patronymic, Phone, Description, IsLocked, rg.RightID, rg.RightName, a.ApplicationID, a.ApplicationName
	FROM dbo.Users u
		INNER JOIN dbo.UsersAndRoles ur ON u.UserID = ur.UserID
		INNER JOIN dbo.Roles r ON ur.RoleID = r.RoleID
		INNER JOIN dbo.RolesAndRights rr ON r.RoleID = rr.RoleID
		INNER JOIN dbo.Rights rg ON rr.RightID = rg.RightID
		INNER JOIN dbo.Applications a ON rg.ApplicationID = a.ApplicationID
	WHERE Login = @login
		AND Password = @password

)