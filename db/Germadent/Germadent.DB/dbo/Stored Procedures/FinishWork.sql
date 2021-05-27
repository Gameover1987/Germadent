-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 25.05.2021
-- Description:	Подтверждение выполнения работы
-- =============================================
CREATE PROCEDURE [FinishWork] 
	
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
		
		EXEC dbo.ChangeStatusWorkOrder @workOrderId, @userId, @technologyOperationId, @statusNext output, @statusChangeDateTime output

	COMMIT


END