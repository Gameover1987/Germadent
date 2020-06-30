-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 06.04.2020
-- Description:	Добавление или изменение набора качественных атрибутов заказ-наряда
-- =============================================
CREATE PROCEDURE [dbo].[AddOrUpdateAttributesSet]
	
	@jsonString varchar(MAX)

AS
BEGIN
	
	SET NOCOUNT ON;

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

	-- Чистим набор атрибутов от старого содержимого
	DELETE
	FROM AttributesSet
	WHERE WorkOrderID = @workOrderId

	-- Наполняем набор новым содержимым, распарсив строку json
	INSERT INTO AttributesSet
		(WorkOrderID, ToothNumber, AttributeID, AttrValueID)
	SELECT WorkOrderID, ToothNumber, AttributeID, AttrValueID
		FROM OPENJSON (@jsonString)
		WITH (WorkOrderID int, ToothNumber tinyint, AttributeID int, AttrValueID int)

	-- Удаляем незначащие записи
	DELETE
	FROM AttributesSet
	WHERE WorkOrderID = @workOrderId
	AND AttributeID IS NULL

END