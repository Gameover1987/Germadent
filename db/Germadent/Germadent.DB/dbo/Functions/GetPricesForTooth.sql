-- =============================================
-- Author:		 Алексей Колосенок
-- Create date:  29.06.2020
-- Editing date: 09.10.2020
-- Description:	 Возвращает цену изготовления для вставки в зубную карту для зуба
-- =============================================
CREATE FUNCTION [dbo].[GetPricesForTooth]
(	
	@branchTypeId int, 
	@pricePositionId int,
	@stlExist bit = 0
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT pp.PricePositionID,
			CASE -- Соображаем, какую цену услуги выбрать в данном конкретном случае. Сначала определяемся с филиалом:
				WHEN @branchTypeId = 2 THEN pdl.Price
				WHEN @branchTypeId = 1 THEN CASE -- Затем для ФЦ смотрим, есть ли STL-файл и содержит ли услуга цену для STL-варианта:
												WHEN @stlExist = 1 AND pmc.PriceSTL > 0 THEN pmc.PriceSTL
												ELSE pmc.PriceModel
											END
			END Price

	FROM PricePositions pp
		LEFT JOIN PricesDL pdl ON pp.PricePositionID = pdl.PricePositionID
		LEFT JOIN PricesMC pmc ON pp.PricePositionID = pmc.PricePositionID
	WHERE pp.PricePositionID = @pricePositionId
		-- Подтягиваем актуальные цены:
		AND ((GETDATE() BETWEEN pdl.DateBegin AND ISNULL(pdl.DateEnd, '99991231')) OR (GETDATE() BETWEEN pmc.DateBegin AND ISNULL(pmc.DateEnd, '99991231')))
)