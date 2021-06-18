-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 24.05.2020
-- Description:	Изменение статуса заказ-наряда
-- =============================================
CREATE PROCEDURE [dbo].[ChangeStatusWorkOrder] 
	
	@workOrderId int,
	@userId int,
	@technologyOperationId int,
	@statusNext int output,
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
		
	ELSE BEGIN

		BEGIN TRAN

		-- Вычисляем следующий статус после выполнения очередной технологической операции
		SELECT @statusNext = StatusTo
		FROM StatusRoute
		WHERE TechnologyOperationID = @technologyOperationId
		
		IF @statusNext IS NULL BEGIN		
			SELECT @statusNext = StatusTo
			FROM StatusRoute
			WHERE StatusFrom = @currentStatusWO
				AND EmployeePositionID = (SELECT EmployeePositionID
											FROM dbo.TechnologyOperations
											WHERE TechnologyOperationID = @technologyOperationId)
				AND TechnologyOperationID IS NULL
		END

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
    ON OBJECT::[dbo].[ChangeStatusWorkOrder] TO [gdl_user]
    AS [dbo];

