﻿-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 06.04.2020
-- Description:	Добавление или изменение набора качественных атрибутов заказ-наряда
-- =============================================
CREATE PROCEDURE [dbo].[AddOrUpdateAttributesSet]
	
	@workOrderId int,
	@jsonStringAttributes varchar(MAX)

AS
BEGIN
	
	SET NOCOUNT, XACT_ABORT ON;

    
	-- Если заказ-наряд закрыт - никаких дальнейших действий
	IF((SELECT Status FROM WorkOrder WHERE WorkOrderID = @workOrderId) = 9)
		BEGIN
			RETURN
		END

	BEGIN TRAN
		-- Чистим набор атрибутов от старого содержимого
		DELETE
		FROM AttributesSet
		WHERE WorkOrderID = @workOrderId

		-- Наполняем набор новым содержимым, распарсив строку json
		INSERT INTO AttributesSet
			(WorkOrderID, ToothNumber, AttributeID, AttributeValueID)
		SELECT WorkOrderID = @workOrderId, ToothNumber, AttributeID, AttributeValueID
			FROM OPENJSON (@jsonStringAttributes)
			WITH (ToothNumber tinyint, AttributeId int, AttributeValueId int)

	COMMIT

	-- Удаляем незначащие записи
	DELETE
	FROM AttributesSet
	WHERE WorkOrderID = @workOrderId
	AND AttributeID IS NULL

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddOrUpdateAttributesSet] TO [gdl_user]
    AS [dbo];

