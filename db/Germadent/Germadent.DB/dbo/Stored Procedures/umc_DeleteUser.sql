-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 19.10.2020
-- Description:	Удаление пользователя
-- =============================================
CREATE PROCEDURE [dbo].[umc_DeleteUser] 
	
	@userId int
--	@deletedRows int output

AS
BEGIN
	
	SET NOCOUNT ON;

	DELETE
	FROM dbo.Users
	WHERE UserID = @userId
    
--	SET @deletedRows = @@rowcount
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[umc_DeleteUser] TO [gdl_user]
    AS [dbo];

