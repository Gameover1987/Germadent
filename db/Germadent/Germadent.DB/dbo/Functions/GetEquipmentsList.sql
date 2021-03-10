-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 23.11.2019
-- Description:	Возвращает список оснастки
-- =============================================
CREATE FUNCTION [dbo].[GetEquipmentsList] 
(	
	
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT EquipmentID, EquipmentName
	FROM dbo.Equipments
)