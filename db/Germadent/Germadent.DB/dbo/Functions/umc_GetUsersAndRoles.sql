-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 19.10.2020
-- Description:	Возвращает пользователей (сотрудников), роли
-- =============================================
CREATE FUNCTION [dbo].[umc_GetUsersAndRoles] 
(	
	@userId int	= NULL
)
RETURNS TABLE 
AS
RETURN 
(
	
	SELECT u.UserID, u.Login, u.Password, u.IsLocked, u.Description, r.RoleID, r.RoleName, u.FamilyName, u.FirstName, u.Patronymic, u.Phone, epc.EmployeePositionID, epc.QualifyingRank
	FROM dbo.Users u 
		INNER JOIN dbo.UsersAndRoles ur ON u.UserID = ur.UserID
		INNER JOIN dbo.Roles r ON r.RoleID = ur.RoleID
		LEFT JOIN dbo.EmployeePositionsCombination epc ON u.UserID = epc.EmployeeID
	WHERE u.UserID = ISNULL(@userId, u.UserID)
)