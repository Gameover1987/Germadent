-- =============================================
-- Author:		 Алексей Колосенок
-- Create date:  29.06.2020
-- Editing date: 21.09.2020
-- Description:	 Возвращает цену изготовления, материал и набор изделий для зуба
-- =============================================
CREATE FUNCTION [dbo].[GetPriceMaterialProductSetForTooth]
(	
	@branchTypeId int, 
	@pricePositionId int,
	@stlExist bit = 0
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT pp.PricePositionID, pp.PricePositionCode, pp.PricePositionName, m.MaterialID, m.MaterialName, p.ProductID, p.ProductName,
			CASE -- Соображаем, какую цену услуги выбрать в данном конкретном случае. Сначала определяемся с филиалом:
				WHEN @branchTypeId = 2 THEN pdl.Price
				WHEN @branchTypeId = 1 THEN CASE -- Затем для ФЦ смотрим, есть ли STL-файл и содержит ли услуга цену для STL-варианта:
												WHEN @stlExist = 1 AND pmc.PriceSTL > 0 THEN pmc.PriceSTL
												ELSE pmc.PriceModel
											END
			END Price

	FROM PricePositions pp
		LEFT JOIN Materials m ON pp.MaterialID = m.MaterialID
		INNER JOIN ProductSet ps ON pp.PricePositionID = ps.PricePositionID
		INNER JOIN Product p ON ps.ProductID = p.ProductID
		LEFT JOIN PricesDL pdl ON pp.PricePositionID = pdl.PricePositionID
		LEFT JOIN PricesMC pmc ON pp.PricePositionID = pmc.PricePositionID
	WHERE pp.PricePositionID = @pricePositionId
		AND pdl.DateEnd IS NULL
		AND pmc.DateEnd IS NULL
		AND (m.FlagUnused IS NULL OR m.FlagUnused = 0)
)