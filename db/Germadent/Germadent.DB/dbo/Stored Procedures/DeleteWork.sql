-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 17.05.2021
-- Description:	Удаление работы
-- =============================================
CREATE PROCEDURE [dbo].[DeleteWork] 
	
	@workId int,
	@rowCountResult int output

AS
BEGIN
	
	SET NOCOUNT, XACT_ABORT ON;

    -- Если заказ-наряд закрыт - никаких дальнейших действий
	IF EXISTS (SELECT 1 FROM dbo.StatusList WHERE WorkOrderID IN (SELECT WorkOrderID FROM dbo.WorkList WHERE WorkID = @workId) AND Status = 100)
		RETURN

	DECLARE @workStarted datetime,
			@workCompleted datetime

	SELECT @workStarted = WorkStarted FROM dbo.WorkList WHERE WorkID = @workId
	SELECT @workCompleted = WorkCompleted FROM dbo.WorkList WHERE WorkID = @workId

	-- Проверяем, нет ли попытки удалить завершённую работу
	IF @workCompleted IS NOT NULL AND @workCompleted > @workStarted
		RETURN
	ELSE BEGIN
		DELETE
		FROM dbo.WorkList
		WHERE WorkID = @workId

		SET @rowCountResult = @@ROWCOUNT
		END
END