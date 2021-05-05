﻿-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 20.04.2021
-- Description:	Добавление/изменение работ для заказ-наряда
-- =============================================
CREATE PROCEDURE [dbo].[AddOrUpdateWorkList] 
	
	@workOrderId int,
	@jsonWorklistString nvarchar(MAX)

AS
BEGIN
	
	SET NOCOUNT, XACT_ABORT ON;

    
	-- Если заказ-наряд закрыт - никаких дальнейших действий
	IF EXISTS (SELECT 1 FROM dbo.StatusList WHERE WorkOrderID = @workOrderId AND Status = 9)
		RETURN
		
	BEGIN TRAN
		-- Чистим список работ от старого содержимого
		DELETE
		FROM dbo.WorkList
		WHERE WorkOrderID = @workOrderId

		-- Наполняем новым содержимым, распарсив строку json
		INSERT INTO dbo.WorkList
		(WorkOrderID, ProductID, TechnologyOperationID, EmployeeID, Rate, Quantity, Started, Ended, IsChecked)
		SELECT WorkOrderID = @workOrderId, ProductID, TechnologyOperationID, EmployeeID, Rate, Quantity, Started, Ended, IsChecked
		FROM OPENJSON (@jsonWorklistString)
			WITH(ProductId int, TechnologyOperationId int, EmployeeId int, Rate money, Quantity int, Started datetime, Ended datetime, IsChecked bit)

	COMMIT

	-- Удаляем незначащие записи
	DELETE
	FROM dbo.WorkList
	WHERE WorkOrderID = @workOrderId
	AND TechnologyOperationID IS NULL

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddOrUpdateWorkList] TO [gdl_user]
    AS [dbo];
