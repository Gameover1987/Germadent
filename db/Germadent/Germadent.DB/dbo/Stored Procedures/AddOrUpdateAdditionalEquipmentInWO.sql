-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 01.02.2020
-- Description:	Добавление и изменение оснастки от заказчика
-- =============================================
CREATE PROCEDURE [dbo].[AddOrUpdateAdditionalEquipmentInWO] 
	
	@workOrderId int,
	@jsonEquipmentsString varchar(MAX)

AS
BEGIN
	
	SET NOCOUNT, XACT_ABORT ON;
		
	-- Если заказ-наряд закрыт - никаких дальнейших действий
	IF((SELECT Status FROM dbo.WorkOrder WHERE WorkOrderID = @workOrderId) = 9)
		BEGIN
			RETURN
		END

	BEGIN TRAN
		-- Чистим набор от старого содержимого
		DELETE
		FROM dbo.AdditionalEquipment
		WHERE WorkOrderID = @workOrderId

		-- Наполняем новым содержимым, распарсив строку json
		INSERT INTO dbo.AdditionalEquipment
		(WorkOrderID, EquipmentID, QuantityIn, QuantityOut)
		SELECT WorkOrderID = @workOrderId, EquipmentID, QuantityIn, QuantityOut
		FROM OPENJSON (@jsonEquipmentsString)
			WITH (EquipmentId int, QuantityIn int, QuantityOut int)
    
	COMMIT

	-- Удаляем незначащие записи
	DELETE
	FROM dbo.AdditionalEquipment
	WHERE WorkOrderID = @workOrderId
	AND (QuantityIn = 0 OR QuantityIn IS NULL)
	AND (QuantityOut = 0 OR QuantityOut IS NULL)

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddOrUpdateAdditionalEquipmentInWO] TO [gdl_user]
    AS [dbo];

