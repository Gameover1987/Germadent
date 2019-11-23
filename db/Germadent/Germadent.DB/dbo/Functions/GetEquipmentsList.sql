-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 23.11.2019
-- Description:	Справочник способов протезирования
-- =============================================
CREATE FUNCTION [GetEquipmentsList] 
(	
	
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT EquipmentID, EquipmentName
	FROM Equipments
)