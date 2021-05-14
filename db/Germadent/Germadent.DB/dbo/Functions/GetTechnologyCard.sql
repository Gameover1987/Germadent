-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 11.05.2021
-- Description:	Возвращает набор операций для изделия, входящего в ценовую позицию
-- =============================================
CREATE FUNCTION GetTechnologyCard 
(	
	@pricePositionId int
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT *
	FROM dbo.TechnologyCard
	WHERE PricePositionID = @pricePositionId
)