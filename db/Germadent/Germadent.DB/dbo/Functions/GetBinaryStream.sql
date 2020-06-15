-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 19.02.2020
-- Description:	Возвращает двоичный поток для заданного workOrderId. Работает, если к 1 заказ-наряду прицеплен 1 файл
-- =============================================
CREATE FUNCTION [dbo].[GetBinaryStream]
(
	@workOrderId int
-- , @streamId uniqueidentifier
)
RETURNS varbinary(max)
AS
BEGIN
	DECLARE 
	@fileStream varbinary(max)

	SELECT @fileStream = file_stream
	FROM StlAndPhotos
	WHERE stream_id = (SELECT stream_id FROM LinksFileStreams WHERE WorkOrderID = @workOrderId)

	RETURN @fileStream

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBinaryStream] TO [gdl_user]
    AS [dbo];

