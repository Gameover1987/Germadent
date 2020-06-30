-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 30.06.2020
-- Description:	Возвращает справочник цветов конструкции
-- =============================================
CREATE FUNCTION GetConstructionColorsList
(	
	
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT ConstructionColorID, ConstructionColorName
	FROM ConstructionColors
)