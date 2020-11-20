-- =============================================
-- Author:		 Алексей Колосенок
-- Create date:  19.02.2020
-- Editing date: 24.10.2020
-- Description:	 Возвращает атрибуты файловых потоков для пользователя
-- =============================================
CREATE FUNCTION [dbo].[GetFileAttributesByUserId]
(	
	 @userId int
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT lfs.UserID, lfs.stream_id, p.name, p.cached_file_size, p.creation_time, p.last_write_time
	FROM LinksFileStreams lfs INNER JOIN Pictures p ON lfs.stream_id = p.stream_id
	WHERE lfs.UserID = @userId
)