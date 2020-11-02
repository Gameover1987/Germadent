-- =============================================
-- Author:		 Алексей Колосенок
-- Create date:  29.06.2020
-- Editing date: 10.10.2020
-- Description:	 Возвращает актуальную цену изготовления для вставки в зубную карту для зуба
-- =============================================
CREATE FUNCTION [dbo].[GetActualPriceForTooth]
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
				WHEN @branchTypeId = 2 THEN p.PriceModel
				WHEN @branchTypeId = 1 THEN CASE -- Затем для ФЦ смотрим, есть ли STL-файл и содержит ли услуга цену для STL-варианта:
												WHEN @stlExist = 1 AND p.PriceSTL > 0 THEN p.PriceSTL
												ELSE p.PriceModel
											END
			END Price

	FROM PricePositions pp
		LEFT JOIN Prices p ON pp.PricePositionID = p.PricePositionID
	WHERE pp.PricePositionID = @pricePositionId
		-- Подтягиваем актуальные цены:
		AND GETDATE() BETWEEN p.DateBegin AND ISNULL(p.DateEnd, '99991231')
)