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

	-- Если заказ-наряд закрыт - никаких дальнейших действий
	IF((SELECT Status FROM WorkOrder WHERE WorkOrderID = @workOrderId) = 2)
		BEGIN
			RETURN
		END

	-- Чистим зубную карту от старого содержимого
	DELETE
	FROM ToothCard
	WHERE WorkOrderID = @workOrderId

	-- Наполняем новым содержимым, распарсив строку json
	INSERT INTO ToothCard
		(WorkOrderID, ToothNumber, ConditionID, ProstheticsID, MaterialID, FlagBridge)
	SELECT WorkOrderID, ToothNumber, ConditionID, ProstheticsID, MaterialID, HasBridge
	FROM OPENJSON (@jsonString)
		WITH (WorkOrderId int, ToothNumber int, ConditionId int, ProstheticsId int, MaterialId int, HasBridge bit)

	-- Удаляем незначащие записи
	DELETE
	FROM ToothCard
	WHERE WorkOrderID = @workOrderId
	AND ConditionID IS NULL
	AND ProstheticsID IS NULL
	AND MaterialID IS NULL	

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddOrUpdateToothCardInWO] TO [gdl_user]
    AS [dbo];

