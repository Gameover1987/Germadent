-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 08.02.2020
-- Description:	Добавление связи пользователя и файлового хранилища
-- =============================================
CREATE PROCEDURE [dbo].[AddLink_User_FileStream] 
	
	@fileName nvarchar(70),
	@creationTime datetimeoffset,
	@userId int
	
AS
BEGIN
	
	SET NOCOUNT, XACT_ABORT ON;

	DECLARE @streamId uniqueidentifier



	-- Получение id файлового потока по имени файла и времени его создания
	SET @streamId = (SELECT stream_id FROM Pictures	WHERE name = @fileName	AND creation_time = @creationTime)

	-- Проверка, если такая связь уже есть
	IF EXISTS (SELECT stream_id FROM LinksFileStreams WHERE stream_id = @streamId AND UserID = @userId)
		RETURN
	
	ELSE BEGIN TRAN
		DELETE FROM LinksFileStreams 
		WHERE UserID = @userId

		-- Собственно вставка
		INSERT INTO LinksFileStreams
		(UserID, stream_id)
		VALUES
		(@userId, @streamId)

		-- Удаление "бесхозных" картинок
		DELETE FROM Pictures
		WHERE stream_id NOT IN (SELECT stream_id FROM LinksFileStreams)
		
	COMMIT

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddLink_User_FileStream] TO [gdl_user]
    AS [dbo];

