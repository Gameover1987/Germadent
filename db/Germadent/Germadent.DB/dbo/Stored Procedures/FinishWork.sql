-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 25.05.2021
-- Description:	Подтверждение выполнения работы
-- =============================================
CREATE PROCEDURE [dbo].[FinishWork] 
	
	@workId int,
	@userId int,
	@statusChangeDateTime datetime output
	

AS
BEGIN
	
	SET NOCOUNT, XACT_ABORT ON;

	--DECLARE
	--@statusNext int

	BEGIN TRAN
		SET @statusChangeDateTime = GETDATE()

		UPDATE dbo.WorkList
		SET WorkCompleted = @statusChangeDateTime
			, LastEditor = @userId
		WHERE WorkID = @workId

		--IF NOT EXISTS
		--	(SELECT 1
		--	FROM dbo.TechnologyOperations
		--	WHERE TechnologyOperationID = @technologyOperationId
		--		AND EmployeePositionID IN (SELECT teop.EmployeePositionID
		--									FROM dbo.WorkList wl INNER JOIN dbo.TechnologyOperations teop ON wl.TechnologyOperationID = teop.TechnologyOperationID
		--									WHERE wl.WorkOrderID = @workOrderId
		--										AND wl.TechnologyOperationID = @technologyOperationId
		--										AND wl.WorkCompleted IS NULL))
		--	EXEC dbo.ChangeStatusWorkOrder @workOrderId, @userId, @technologyOperationId, @statusNext output, @statusChangeDateTime output

	COMMIT

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[FinishWork] TO [gdl_user]
    AS [dbo];

