-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 01.12.2019
-- Description:	Добавление и редактирование зубной карты в заказ-наряде
-- =============================================
CREATE PROCEDURE [dbo].[AddOrUpdateToothCardInWO] 
	
	@jsonString varchar(MAX)

AS
BEGIN
	
	-- Достаём нужный id
	DECLARE @workOrderId int

	SET 	 @workOrderId = (SELECT DISTINCT WorkOrderID
							FROM OPENJSON (@jsonString)
								WITH (WorkOrderId int))

	-- Чистим зубную карту от старого содержимого
	DELETE
	FROM ToothCard
	WHERE WorkOrderID = @workOrderId

	-- Наполняем новым содержимым, распарсив строку json
	INSERT INTO ToothCard
		(WorkOrderID, ToothNumber, MaterialID, ProstheticsID, FlagBridge)
	SELECT WorkOrderID, ToothNumber, MaterialID, ProstheticsID, HasBridge
	FROM OPENJSON (@jsonString)
		WITH (WorkOrderId int, ToothNumber int, MaterialId int, ProstheticsId int, HasBridge bit)	

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddOrUpdateToothCardInWO] TO [gdl_user]
    AS [dbo];

