-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 17.05.2021
-- Description:	Редактирование работы в заказ-наряде
-- =============================================
CREATE PROCEDURE UpdateWork 
	
	@workId int,
	@workOrderId int,
	@quantity int,
	@operationCost money,
	@workCompleted datetime,
	@remark nvarchar(250),
	@userId int
	
AS
BEGIN
	
	SET NOCOUNT, XACT_ABORT ON;

    -- Если заказ-наряд закрыт - никаких дальнейших действий
	IF EXISTS (SELECT 1 FROM dbo.StatusList WHERE WorkOrderID = @workOrderId AND Status = 100)
		RETURN
	
	BEGIN TRAN
		
		UPDATE dbo.WorkList
		SET	Quantity = @quantity
		, OperationCost = @operationCost
		, WorkCompleted = @workCompleted
		, Remark = @remark
		, LastEditor = @userId

	COMMIT

END