-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 19.02.2020
-- Description:	Возвращает двоичный поток для заданного userId. Работает, если к 1 пользователю прицеплена 1 фотография
-- =============================================
CREATE FUNCTION [dbo].[GetBinaryStream]
(
	@userId int
-- , @streamId uniqueidentifier
)
RETURNS varbinary(max)
AS
BEGIN
	DECLARE 
	@fileStream varbinary(max)

	SELECT @fileStream = file_stream
	FROM StlAndPhotos
	WHERE stream_id = (SELECT stream_id FROM LinksFileStreams WHERE UserID = @userId)

	RETURN @fileStream

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBinaryStream] TO [gdl_user]
    AS [dbo];

