-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 06.04.2020
-- Description:	Добавление или изменение набора качественных атрибутов заказ-наряда
-- =============================================
CREATE PROCEDURE [dbo].[AddOrUpdateQualitAttributesSet]
	
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
	FROM QualitAttributesSet
	WHERE WorkOrderID = @workOrderId

	-- Наполняем набор новым содержимым, распарсив строку json
	INSERT INTO QualitAttributesSet
		(WorkOrderID, AttributeID, QValueID)
	SELECT WorkOrderID, AttributeID, QValueID
		FROM OPENJSON (@jsonString)
		WITH (WorkOrderID int, AttributeID int, QValueID int)

	-- Удаляем незначащие записи
	DELETE
	FROM QualitAttributesSet
	WHERE WorkOrderID = @workOrderId
	AND QValueID IS NULL

END