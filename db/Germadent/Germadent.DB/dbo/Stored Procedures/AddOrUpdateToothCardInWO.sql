-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 01.12.2019
-- Description:	Добавление и редактирование зубной карты в заказ-наряде
-- =============================================
CREATE PROCEDURE [dbo].[AddOrUpdateToothCardInWO] 
	
	@jsonString varchar(MAX)

AS
BEGIN
	
	DECLARE @workOrderId int

	SET 	 @workOrderId = (SELECT DISTINCT  WorkOrderID
							FROM OPENJSON (@jsonString)
								WITH (workOrderID int))

	DELETE
	FROM ToothCard
	WHERE WorkOrderID = @workOrderId

	INSERT INTO ToothCard
		(WorkOrderID, ToothNumber, MaterialID, ProstheticsID, FlagBridge)
	SELECT WorkOrderID, ToothNumber, MaterialID, ProstheticsID, FlagBridge
	FROM OPENJSON (@jsonString)
		WITH (workOrderID int, toothNumber int, materialID int, prostheticsID int, flagBridge bit)	

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddOrUpdateToothCardInWO] TO [gdl_user]
    AS [dbo];

