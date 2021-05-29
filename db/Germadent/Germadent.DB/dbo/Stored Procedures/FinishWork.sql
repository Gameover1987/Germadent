-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 25.05.2021
-- Description:	Подтверждение выполнения работы
-- =============================================
CREATE PROCEDURE [dbo].[FinishWork] 
	
	@workId int,
	@workOrderId int,
	@technologyOperationId int,
	@userId int
	

AS
BEGIN
	
	SET NOCOUNT, XACT_ABORT ON;

	DECLARE
	@statusNext int, 
	@statusChangeDateTime datetime

	BEGIN TRAN
		
		UPDATE dbo.WorkList
		SET WorkCompleted = GETDATE()

		IF NOT EXISTS
			(SELECT 1
			FROM dbo.TechnologyOperations
			WHERE TechnologyOperationID = @technologyOperationId
				AND EmployeePositionID IN (SELECT teop.EmployeePositionID
											FROM dbo.WorkList wl INNER JOIN dbo.TechnologyOperations teop ON wl.TechnologyOperationID = teop.TechnologyOperationID
											WHERE wl.WorkOrderID = @workOrderId
												AND wl.TechnologyOperationID = @technologyOperationId
												AND wl.WorkCompleted IS NULL))
			EXEC dbo.ChangeStatusWorkOrder @workOrderId, @userId, @technologyOperationId, @statusNext output, @statusChangeDateTime output

	COMMIT

END