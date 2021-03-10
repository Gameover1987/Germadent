-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 19.10.2020
-- Description:	Добавление пользователя
-- =============================================
CREATE PROCEDURE [dbo].[umc_AddUser] 
	
	@familyName nvarchar(MAX),
	@firstName nvarchar(MAX),
	@patronymic nvarchar(MAX) = NULL,
	@phone nvarchar(MAX) = NULL,
	@login nvarchar(100),
	@password nvarchar(MAX),
	@isLocked bit = NULL,
	@description nvarchar(MAX) = NULL,
	@jsonString varchar(MAX),
	@userId int output

AS
BEGIN
	
	SET NOCOUNT, XACT_ABORT ON;

	-- Чтобы неоправданно не возрастало значение Id в ключевом поле - сначала его "подбивка":
	BEGIN
		DECLARE @max_Id int
		SELECT @max_Id = ISNULL(MAX(UserID), 0)
		FROM dbo.Users

		EXEC dbo.IdentifierAlignment Users, @max_Id
	
		REVERT
	END
    
	BEGIN TRAN
		INSERT INTO dbo.Users
		(Login, Password, FamilyName, FirstName, Patronymic, Phone, IsLocked, Description)
		VALUES
		(@login, @password, @familyName, @firstName, @patronymic, @phone, @isLocked, @description)

		SET @userId = SCOPE_IDENTITY()

		-- Чистим набор ролей от старого содержимого
		DELETE
		FROM dbo.UsersAndRoles
		WHERE UserID = @userId

		-- Наполняем набор новым содержимым, распарсив строку json
		INSERT INTO dbo.UsersAndRoles
		(UserID, RoleID)
		SELECT UserID = @userId, RoleID
		FROM OPENJSON(@jsonString)
		WITH(RoleId int)
	COMMIT

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[umc_AddUser] TO [gdl_user]
    AS [dbo];

