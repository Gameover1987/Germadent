-- =============================================
-- Author:		 Алексей Колосенок
-- Create date:  01.12.2019
-- Editing date: 01.07.2020
-- Description:	 Добавление и редактирование зубной карты в заказ-наряде
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
	IF((SELECT Status FROM WorkOrder WHERE WorkOrderID = @workOrderId) = 9)
		BEGIN
			RETURN
		END

	-- Чистим зубную карту от старого содержимого
	DELETE
	FROM ToothCard
	WHERE WorkOrderID = @workOrderId

	-- Наполняем новым содержимым, распарсив строку json
	INSERT INTO ToothCard
		(WorkOrderID, ToothNumber, PricePositionID, ConditionID, MaterialID, ProductID, Price, HasBridge)
	SELECT WorkOrderID, ToothNumber, PricePositionID, ConditionID, MaterialID, ProductID, Price, HasBridge
	FROM OPENJSON (@jsonString)
		WITH (WorkOrderId int, ToothNumber int, PricePositionId int, ConditionId int, MaterialId int, ProductId int, Price money, HasBridge bit)

	-- Удаляем незначащие записи
	DELETE
	FROM ToothCard
	WHERE WorkOrderID = @workOrderId
	AND PricePositionID IS NULL
	
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddOrUpdateToothCardInWO] TO [gdl_user]
    AS [dbo];

