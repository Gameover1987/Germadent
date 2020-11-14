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
	SELECT lfs.UserID, lfs.stream_id, s.name, s.cached_file_size, s.creation_time, s.last_write_time
	FROM LinksFileStreams lfs INNER JOIN StlAndPhotos s ON lfs.stream_id = s.stream_id
	WHERE lfs.UserID = @userId
)