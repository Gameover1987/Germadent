-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 08.11.2020
-- Description:	Возвращает все цены для ценовой позиции
-- =============================================
CREATE FUNCTION GetAllPricesForPricePosition 
(	
	@pricePositionId int
	
)
RETURNS TABLE 
AS
RETURN 
(
	
	SELECT *
	FROM Prices
	WHERE PricePositionID = @pricePositionId

)