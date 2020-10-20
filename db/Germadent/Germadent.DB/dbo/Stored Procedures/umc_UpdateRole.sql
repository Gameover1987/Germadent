-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 18.10.2020
-- Description:	Редактирование роли
-- =============================================
CREATE PROCEDURE [dbo].[umc_UpdateRole] 
	
	@roleId int, 
	@roleName nvarchar(MAX),
	@jsonString varchar(MAX)

AS
BEGIN
	
	SET NOCOUNT ON;

	UPDATE Roles
	SET RoleName = @roleName
	WHERE RoleID = @roleId

	-- Чистим набор прав от старого содержимого
	DELETE
	FROM RolesAndRights
	WHERE RoleID = @roleId

	-- Наполняем набор новым содержимым, распарсив строку json

	INSERT INTO RolesAndRights
	(RoleID, RightID)
	SELECT RoleID, RightID
	FROM OPENJSON (@jsonString)
	WITH(RoleId int, RightId int)

   
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[umc_UpdateRole] TO [gdl_user]
    AS [dbo];

