-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 20.04.2021
-- Description:	Добавление/изменение работ для заказ-наряда
-- =============================================
CREATE PROCEDURE [dbo].[AddOrUpdateWorkList] 
	
	@workOrderId int,
	@jsonWorklistString nvarchar(MAX)

AS
BEGIN
	
	SET NOCOUNT, XACT_ABORT ON;

    
	-- Если заказ-наряд закрыт - никаких дальнейших действий
	IF EXISTS (SELECT 1 FROM dbo.StatusList WHERE WorkOrderID = @workOrderId AND Status = 100)
		RETURN
		
	DECLARE
	@employeeId int

	SELECT @employeeId = UserId
	FROM OPENJSON(@jsonWorklistString)
	WITH(UserId int)

	BEGIN TRAN
		-- Чистим список работ от старого содержимого
		DELETE
		FROM dbo.WorkList
		WHERE WorkOrderID = @workOrderId
			AND EmployeeID = @employeeId

		-- Наполняем новым содержимым, распарсив строку json
		INSERT INTO dbo.WorkList
		(WorkOrderID, ProductID, TechnologyOperationID, EmployeeID, Rate, Quantity, OperationCost, WorkStarted, WorkCompleted)
		SELECT WorkOrderID = @workOrderId, ProductID, TechnologyOperationID, UserId, Rate, Quantity, OperationCost, CAST(CAST(WorkStarted as datetimeoffset) as datetime), CAST(CAST(WorkCompleted as datetimeoffset) as datetime)
		FROM OPENJSON (@jsonWorklistString)
			WITH(ProductId int, TechnologyOperationId int, UserId int, Rate money, Quantity int, OperationCost money, WorkStarted datetimeoffset, WorkCompleted datetimeoffset)

	COMMIT

	-- Удаляем незначащие записи
	DELETE
	FROM dbo.WorkList
	WHERE WorkOrderID = @workOrderId
	AND TechnologyOperationID IS NULL

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddOrUpdateWorkList] TO [gdl_user]
    AS [dbo];

