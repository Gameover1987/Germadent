-- =============================================
-- Author:		Name
-- Create date: 17.05.2021
-- Description:	Добавление работы в заказ-наряд
-- =============================================
CREATE PROCEDURE [dbo].[AddWork] 
	
	@workOrderId int, 
	@productId int,
	@technologyOperationId int,
	@employeeId int,
	@rate money,
	@quantity int,
	@operationCost money,
	@workStarted datetime,
	@remark nvarchar(250),
	@userId int,
	@workId int output

AS
BEGIN
	
	SET NOCOUNT, XACT_ABORT ON;

	-- Если заказ-наряд закрыт - никаких дальнейших действий
	IF EXISTS (SELECT 1 FROM dbo.StatusList WHERE WorkOrderID = @workOrderId AND Status = 100)
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

		INSERT INTO dbo.WorkList
		(WorkOrderID, ProductID,		TechnologyOperationID, EmployeeID, Rate,		Quantity, OperationCost, WorkStarted,	Remark, LastEditor)
		VALUES
		(@workOrderId, @productId, @technologyOperationId, @employeeId, @rate, @quantity, @operationCost, @workStarted, @remark, @userId)

		SET @workId = @@ROWCOUNT

	COMMIT

END