-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 11.03.2021
-- Description:	Возвращает наборы материалов для ценовых позиций
-- =============================================
CREATE FUNCTION dbo.GetMaterialSetForPricePositions
(	
	 
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT ms.*, MaterialName
	FROM dbo.MaterialSet ms 
	INNER JOIN Materials m ON ms.MaterialID = m.MaterialID
)