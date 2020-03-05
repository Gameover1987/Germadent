-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 08.02.2020
-- Description:	Добавление связи заказ-наряда и файлового хранилища
-- =============================================
CREATE PROCEDURE [dbo].[AddLinkWO_FileStream] 
	
	@fileName nvarchar(70),
	@creationTime datetimeoffset,
	@workOrderId int
	
AS
BEGIN
	
	SET NOCOUNT ON;

	DECLARE @streamId uniqueidentifier

	-- Если заказ-наряд закрыт - никаких дальнейших действий
	IF((SELECT Status FROM WorkOrder WHERE WorkOrderID = @workOrderId) = 9)
		BEGIN
			RETURN
		END

	-- Получение id файлового потока по имени файла и времени его создания
	SET @streamId = (SELECT stream_id FROM StlAndPhotos	WHERE name = @fileName	AND creation_time = @creationTime)

	-- Проверка, если такая связь уже есть
	IF EXISTS (SELECT stream_id FROM LinksFileStreams WHERE stream_id = @streamId AND WorkOrderID = @workOrderId)
	BEGIN
		RETURN
	END

	-- Собственно вставка
	INSERT INTO LinksFileStreams
	(WorkOrderID, stream_id)
	VALUES
    (@workOrderId, @streamId)

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddLinkWO_FileStream] TO [gdl_user]
    AS [dbo];

