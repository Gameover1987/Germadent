-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 11.05.2021
-- Description:	Возвращает набор операций для изделия, входящего в ценовую позицию
-- =============================================
CREATE FUNCTION [dbo].[GetTechnologyCard] 
(	
	@pricePositionId int,
	@workOrderId int
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT tc.PricePositionID, pp.PricePositionCode, teops.*
	FROM dbo.TechnologyCard tc 
		INNER JOIN dbo.TechnologyOperations teops ON tc.TechnologyOperationID = teops.TechnologyOperationID
		INNER JOIN dbo.PricePositions pp ON tc.PricePositionID = pp.PricePositionID
		INNER JOIN dbo.ProductSet ps ON ps.PricePositionID = pp.PricePositionID
		INNER JOIN dbo.Products p ON p.ProductID = ps.ProductID
	WHERE tc.PricePositionID = @pricePositionId
)