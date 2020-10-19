-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 18.10.2020
-- Description:	Добавление роли
-- =============================================
CREATE PROCEDURE umc_AddRole 
	
	@roleName nvarchar(max),
	@roleId int output

AS
BEGIN
	
	SET NOCOUNT ON;

	-- Чтобы неоправданно не возрастало значение Id в ключевом поле - сначала его "подбивка":
	BEGIN
		DECLARE @max_Id int
		SELECT @max_Id = ISNULL(MAX(RoleID), 0)
		FROM Roles

		EXEC IdentifierAlignment Customers, @max_Id
	
		REVERT
	END
	-- Собственно вставка:

    INSERT INTO Roles
	(RoleName)
	VALUES
	(@roleName)

	SET @roleId = SCOPE_IDENTITY()

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[umc_AddRole] TO [gdl_user]
    AS [dbo];

