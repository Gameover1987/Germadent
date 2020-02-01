-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 01.02.2020
-- Description:	Добавление и изменение запчастей от заказчика
-- =============================================
CREATE PROCEDURE [dbo].[AddOrUpdateAdditionalEquipment] 
	
	@jsonEquipments varchar(MAX)

AS
BEGIN
	
	SET NOCOUNT ON;

	-- Достаём нужный id
	DECLARE @workOrderId int

	SET 	 @workOrderId = (SELECT DISTINCT WorkOrderID
							FROM OPENJSON (@jsonEquipments)
								WITH (WorkOrderId int))

	-- Чистим набор от старого содержимого
	DELETE
	FROM AdditionalEquipment
	WHERE WorkOrderID = @workOrderId

	-- Наполняем новым содержимым, распарсив строку json
	INSERT INTO AdditionalEquipment
	(WorkOrderID, EquipmentID, Quantity)
	SELECT WorkOrderID, EquipmentID, Quantity
	FROM OPENJSON (@jsonEquipments)
		WITH (WorkOrderId int, EquipmentId int, Quantity int)
    
	-- Удаляем незначащие записи
	DELETE
	FROM AdditionalEquipment
	WHERE WorkOrderID = @workOrderId
	AND (Quantity = 0 OR Quantity IS NULL)

END