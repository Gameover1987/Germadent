-- =============================================
-- Author:		 Алексей Колосенок
-- Create date:  01.12.2019
-- Editing date: 01.07.2020
-- Description:	 Добавление и редактирование зубной карты в заказ-наряде
-- =============================================
CREATE PROCEDURE [dbo].[AddOrUpdateToothCardInWO] 
	
	@workOrderId int,
	@jsonToothCardString varchar(MAX)

AS
BEGIN
	
	SET NOCOUNT, XACT_ABORT ON;

	-- Если заказ-наряд закрыт - никаких дальнейших действий
	IF EXISTS (SELECT 1 FROM dbo.StatusList WHERE WorkOrderID = @workOrderId AND Status = 100)
		RETURN

	-- Тащим коэффициент срочности
	DECLARE @urgencyRatio float
	SELECT @urgencyRatio = UrgencyRatio 
		FROM dbo.WorkOrder
		WHERE WorkOrderID = @workOrderId

	BEGIN TRAN
		-- Чистим зубную карту от старого содержимого
		DELETE
		FROM dbo.ToothCard
		WHERE WorkOrderID = @workOrderId

		-- Наполняем новым содержимым, распарсив строку json
		INSERT INTO dbo.ToothCard
			(WorkOrderID, ToothNumber, PricePositionID, ConditionID, MaterialID, ProductID, Price, HasBridge)
		SELECT WorkOrderID = @workOrderId, ToothNumber, PricePositionID, ConditionID, MaterialID, ProductID * @urgencyRatio, Price, HasBridge
		FROM OPENJSON (@jsonToothCardString)
			WITH (ToothNumber int, PricePositionId int, ConditionId int, MaterialId int, ProductId int, Price money, HasBridge bit)

	COMMIT

	-- Удаляем незначащие записи
	DELETE
	FROM dbo.ToothCard
	WHERE WorkOrderID = @workOrderId
	AND PricePositionID IS NULL
	
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddOrUpdateToothCardInWO] TO [gdl_user]
    AS [dbo];

