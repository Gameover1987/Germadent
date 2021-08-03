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
	@comment nvarchar(max),
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

		--DECLARE @statusNext int,
		--		@statusChangeDateTime datetime

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
		
		-- Добавляем работу в список
		INSERT INTO dbo.WorkList
		(WorkOrderID, ProductID,		TechnologyOperationID, EmployeeIDStarted, Rate,		Quantity, OperationCost, WorkStarted,	Comment)
		VALUES
		(@workOrderId, @productId, @technologyOperationId, @userIdStarted,	@rate,		@quantity, @operationCost, GETDATE(), @comment)

		SET @workId = SCOPE_IDENTITY()

	COMMIT
	
	-- Закрытие работ моделировщика в ФЦ, когда оператор берёт в работу заказ-наряд
	BEGIN TRAN
		DECLARE @branchTypeID int,
				@flagStl bit,
				@employeePositionId int

		SELECT @branchTypeID = BranchTypeID, @flagStl = FlagStl
		FROM dbo.WorkOrder
		WHERE WorkOrderID = @workOrderId
	
		SELECT @employeePositionId = EmployeePositionID
		FROM dbo.TechnologyOperations
		WHERE TechnologyOperationID = @technologyOperationId
	
		IF @branchTypeID = 1 AND @flagStl = 0 AND @employeePositionId IN (3, 4)
			UPDATE dbo.WorkList
			SET EmployeeIDCompleted = @userIdStarted,
				WorkCompleted = GETDATE()
			WHERE WorkOrderID = @workOrderId
				AND TechnologyOperationID = 1
				AND ProductID = @productId
				AND EmployeeIDCompleted IS NULL
				AND WorkCompleted IS NULL
	COMMIT

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddWork] TO [gdl_user]
    AS [dbo];

