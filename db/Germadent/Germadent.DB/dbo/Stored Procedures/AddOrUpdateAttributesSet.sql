-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 06.04.2020
-- Description:	Добавление или изменение набора качественных атрибутов заказ-наряда
-- =============================================
CREATE PROCEDURE [dbo].[AddOrUpdateAttributesSet]
	
	@workOrderId int,
	@jsonAttributesString varchar(MAX)

AS
BEGIN
	
	SET NOCOUNT, XACT_ABORT ON;

    
	-- Если заказ-наряд закрыт - никаких дальнейших действий
	IF EXISTS (SELECT 1 FROM dbo.StatusList WHERE WorkOrderID = @workOrderId AND Status = 10)
		RETURN

	BEGIN TRAN
		-- Чистим набор атрибутов от старого содержимого
		DELETE
		FROM dbo.AttributesSet
		WHERE WorkOrderID = @workOrderId

		-- Наполняем набор новым содержимым, распарсив строку json
		INSERT INTO dbo.AttributesSet
			(WorkOrderID, ToothNumber, AttributeID, AttributeValueID)
		SELECT WorkOrderID = @workOrderId, ToothNumber, AttributeID, AttributeValueID
			FROM OPENJSON (@jsonAttributesString)
			WITH (ToothNumber tinyint, AttributeId int, AttributeValueId int)

	COMMIT

	-- Удаляем незначащие записи
	DELETE
	FROM dbo.AttributesSet
	WHERE WorkOrderID = @workOrderId
	AND AttributeID IS NULL

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddOrUpdateAttributesSet] TO [gdl_user]
    AS [dbo];

