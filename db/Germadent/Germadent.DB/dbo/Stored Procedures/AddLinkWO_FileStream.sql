-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 08.02.2020
-- Description:	Добавление связи заказ-наряда и файлового хранилища
-- =============================================
CREATE PROCEDURE AddLinkWO_FileStream 
	
	@fileName nvarchar(70),
	@creationTime datetime,
	@workOrderId int
	
AS
BEGIN
	
	SET NOCOUNT ON;

	INSERT INTO FileStreams
	(WorkOrderID, stream_id)
	VALUES
    (@workOrderId, (SELECT stream_id 
					FROM StlAndPhotos	
					WHERE name = @fileName	
						AND creation_time = @creationTime))

END