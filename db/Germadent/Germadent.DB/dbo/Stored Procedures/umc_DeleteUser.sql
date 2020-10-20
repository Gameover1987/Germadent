-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 19.10.2020
-- Description:	Удаление пользователя
-- =============================================
CREATE PROCEDURE [dbo].[umc_DeleteUser] 
	
	@userId int,
	@deletedRows int output

AS
BEGIN
	
	SET NOCOUNT ON;

	DELETE
	FROM Users
	WHERE UserID = userId
    
	SET @deletedRows = @@rowcount
END