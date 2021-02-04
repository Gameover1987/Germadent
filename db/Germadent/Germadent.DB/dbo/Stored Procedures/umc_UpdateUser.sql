-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 19.10.2020
-- Description:	Редактирование пользователя, сотрудника, набора ролей
-- =============================================
CREATE PROCEDURE [dbo].[umc_UpdateUser] 
	
	 @userId int
	, @familyName nvarchar(MAX)
	, @firstName nvarchar(MAX)
	, @patronymic nvarchar(MAX)
	, @phone nvarchar(MAX)
	, @login nvarchar(100)
	, @password nvarchar(MAX)
	, @isLocked bit
	, @description nvarchar(MAX)
	, @jsonString varchar(MAX)
	

AS
BEGIN
	
	SET NOCOUNT, XACT_ABORT ON;

	BEGIN TRAN
		-- Наполняем главную таблицу
    		UPDATE dbo.Users
		SET Login = @login
			, Password = @password
			, FamilyName = @familyName
			, FirstName = @firstName
			, Patronymic = @patronymic
			, Phone = @phone
			, IsLocked = @isLocked
			, Description = @description
		WHERE UserID = @userId

		-- Чистим набор ролей от старого содержимого
		DELETE
		FROM dbo.UsersAndRoles
		WHERE UserID = @userId

		-- Наполняем набор новым содержимым, распарсив строку json
		INSERT INTO dbo.UsersAndRoles
		(UserID, RoleID)
		SELECT UserID, RoleID
		FROM OPENJSON(@jsonString)
		WITH(UserId int, RoleId int)
	COMMIT

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[umc_UpdateUser] TO [gdl_user]
    AS [dbo];

