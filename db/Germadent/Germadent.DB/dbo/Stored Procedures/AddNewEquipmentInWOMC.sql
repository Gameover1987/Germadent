-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 01.12.2019
-- Description:	Добавление оснастки в заказ-наряд ФЦ
-- =============================================
CREATE PROCEDURE [AddNewEquipmentInWOMC] 
	
	@jsonString varchar(MAX)

AS
BEGIN
	
	INSERT INTO AdditionalEquipment
		(WorkOrderMCID, EquipmentID, Quantity)
	SELECT WorkOrderMCID, EquipmentID, Quantity
	FROM OPENJSON (@jsonString)
		WITH (workOrderMCId int, equipmentId int, quantity tinyint)
    
END