-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 19.10.2020
-- Description:	Возвращает пользователей (сотрудников), роли
-- =============================================
CREATE FUNCTION [dbo].[umc_GetUsersAndRoles] 
(	
	
	 
)
RETURNS TABLE 
AS
RETURN 
(
	
	SELECT u.UserID, u.Login, u.Password, u.IsLocked, u.Description, r.RoleID, r.RoleName, u.FamilyName, u.FirstName, u.Patronymic, u.Phone
	FROM Users u 
		INNER JOIN UsersAndRoles ur ON u.UserID = ur.UserID
		INNER JOIN Roles r ON r.RoleID = ur.RoleID
)