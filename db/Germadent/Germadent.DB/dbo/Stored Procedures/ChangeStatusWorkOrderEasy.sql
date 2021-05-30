-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 29.05.2020
-- Description:	Изменение статуса заказ-наряда по-простому
-- =============================================
CREATE PROCEDURE [dbo].[ChangeStatusWorkOrderEasy]

	@workOrderId int,
	@userId int,	
	@statusNext int,
	@statusChangeDateTime datetime output

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

	-- Если заказ-наряд уже закрыт и пользователь не админ - никаких дальнейших действий
	IF @currentStatusWO = 100
		AND (SELECT RoleID FROM dbo.UsersAndRoles WHERE UserID = @userId) != 1
		RETURN
	
	-- При попытке повторно установить тот же статус для заказ-наряда - ничего не делаем
	IF EXISTS (SELECT 1 FROM StatusList 
				WHERE WorkOrderID = @workOrderId 
					AND Status = @statusNext)
		RETURN

	ELSE BEGIN
		BEGIN TRAN

			SET @statusChangeDateTime = GETDATE()

			INSERT INTO dbo.StatusList
			(WorkOrderID, Status, StatusChangeDateTime, UserID)
			VALUES
			(@workOrderId, @statusNext, @statusChangeDateTime, @userId)

		COMMIT
	END

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ChangeStatusWorkOrderEasy] TO [gdl_user]
    AS [dbo];

