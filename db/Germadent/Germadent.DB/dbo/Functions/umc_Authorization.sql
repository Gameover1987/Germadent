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
	FROM Users u
		INNER JOIN UsersAndRoles ur ON u.UserID = ur.UserID
		INNER JOIN Roles r ON ur.RoleID = r.RoleID
		INNER JOIN RolesAndRights rr ON r.RoleID = rr.RoleID
		INNER JOIN Rights rg ON rr.RightID = rg.RightID
		INNER JOIN Applications a ON rg.ApplicationID = a.ApplicationID
	WHERE Login = @login
		AND Password = @password

)