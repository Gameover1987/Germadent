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
	
	SET NOCOUNT, XACT_ABORT ON;

	BEGIN TRAN
		UPDATE dbo.Roles
		SET RoleName = @roleName
		WHERE RoleID = @roleId

		-- Чистим набор прав от старого содержимого
		DELETE
		FROM dbo.RolesAndRights
		WHERE RoleID = @roleId

		-- Наполняем набор новым содержимым, распарсив строку json

		INSERT INTO dbo.RolesAndRights
		(RoleID, RightID)
		SELECT RoleID, RightID
		FROM OPENJSON (@jsonString)
		WITH(RoleId int, RightId int)
	COMMIT
   
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[umc_UpdateRole] TO [gdl_user]
    AS [dbo];

