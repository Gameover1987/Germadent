-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 25.01.2020
-- Description:	Возвращает список типов протезирования
-- =============================================
CREATE FUNCTION [dbo].[GetTypesOfProsthetics]
(	
	
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT ProstheticsID, ProstheticsName
	FROM TypesOfProsthetics 
)