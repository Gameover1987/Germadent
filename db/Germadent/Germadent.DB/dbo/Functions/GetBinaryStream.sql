-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 19.02.2020
-- Description:	Возвращает двоичный поток для заданного stream_id
-- =============================================
CREATE FUNCTION GetBinaryStream
(
	@streamId uniqueidentifier
)
RETURNS varbinary(max)
AS
BEGIN
	DECLARE @fileStream varbinary(max)

	SELECT @fileStream = file_stream
	FROM StlAndPhotos
	WHERE stream_id = @streamId

	RETURN @fileStream

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBinaryStream] TO [gdl_user]
    AS [dbo];

