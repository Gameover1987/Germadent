-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 06.02.2020
-- Description:	Возвращает полный сетевой путь виртуальной папки для файловой таблицы
-- =============================================
CREATE FUNCTION [dbo].[GetFileTableFullPath] 
(
	
	
)
RETURNS varchar(250)
AS
BEGIN
	
	DECLARE @FileTableName varchar(50),
			@FullPath varchar(250)

	
	SET @FileTableName = 'StlAndPhotos'
	SELECT @FullPath = CONCAT(FileTableRootPath(),'\', @FileTableName)

	
	RETURN @FullPath

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFileTableFullPath] TO [gdl_user]
    AS [dbo];

