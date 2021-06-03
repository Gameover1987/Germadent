-- =============================================
-- Author:		Name
-- Create date: 17.05.2021
-- Description:	Добавление работы в заказ-наряд
-- =============================================
CREATE PROCEDURE [dbo].[AddWork] 
	
	@workOrderId int, 
	@productId int,
	@technologyOperationId int,
	@rate money,
	@quantity int,
	@operationCost money,
	@remark nvarchar(250),
	@userIdStarted int,
	@workId int output

AS
BEGIN
	
	SET NOCOUNT, XACT_ABORT ON;

	DECLARE
	@currentStatusWO int

	-- Определяем текущий статус заказ-наряда
	SELECT @currentStatusWO = Status
	FROM dbo.StatusList
	WHERE WorkOrderID = @workOrderId AND StatusChangeDateTime = (SELECT MAX(StatusChangeDateTime)
																	FROM dbo.StatusList
																	WHERE WorkOrderID = @workOrderId)

	-- Если заказ-наряд уже закрыт - никаких дальнейших действий
	IF @currentStatusWO = 100
		RETURN

	BEGIN TRAN

		-- Чтобы неоправданно не возрастало значение Id в ключевом поле - сначала его "подбивка":
		BEGIN
			DECLARE @max_Id int
			SELECT @max_Id = ISNULL(MAX(WorkID), 0)
			FROM dbo.WorkList

			EXEC dbo.IdentifierAlignment WorkList, @max_Id
	
			REVERT
		END

		DECLARE @statusNext int,
				@statusChangeDateTime datetime

		-- Если в заказ-наряде нет подобных работ - переводим статус
		--IF NOT EXISTS
		--	(SELECT 1
		--	FROM dbo.TechnologyOperations
		--	WHERE TechnologyOperationID = @technologyOperationId
		--		AND EmployeePositionID IN (SELECT teop.EmployeePositionID
		--									FROM dbo.WorkList wl INNER JOIN dbo.TechnologyOperations teop ON wl.TechnologyOperationID = teop.TechnologyOperationID
		--									WHERE wl.WorkOrderID = @workOrderId
		--										AND wl.TechnologyOperationID = @technologyOperationId))
		--	EXEC dbo.ChangeStatusWorkOrder @workOrderId, @userId, @technologyOperationId, @statusNext output, @statusChangeDateTime output
		
		-- После этого добавляем работу в список
		INSERT INTO dbo.WorkList
		(WorkOrderID, ProductID,		TechnologyOperationID, EmployeeIDStarted, Rate,		Quantity, OperationCost, WorkStarted,	Remark)
		VALUES
		(@workOrderId, @productId, @technologyOperationId, @userIdStarted,	@rate,		@quantity, @operationCost, GETDATE(), @remark)

		SET @workId = SCOPE_IDENTITY()

	COMMIT

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddWork] TO [gdl_user]
    AS [dbo];

