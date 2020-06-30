-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 29.06.2020
-- Description:	Возвращает название и цену услуги из прайс-листа для зуба
-- =============================================
CREATE FUNCTION [dbo].[GetServicesAndPricesForMaterial]
(	
	@branchTypeId int,
	@materialId int,
	@stlExist bit = 0
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT s.ServiceID, s.PriceGroupID, pg.PriceGroupCode, s.SeviceName,
			CASE -- Соображаем, какую цену услуги выбрать в данном конкретном случае. Сначала определяемся с филиалом:
				WHEN @branchTypeID = 2 THEN pdl.Price
				WHEN @branchTypeID = 1 THEN CASE -- Затем для ФЦ смотрим, есть ли STL-файл и содержит ли услуга цену для STL-варианта:
												WHEN @stlExist = 1 AND pmc.PriceSTL > 0 THEN pmc.PriceSTL
												ELSE pmc.PriceModel
											END
			END Price

	FROM Servicess s
		INNER JOIN PriceGroups pg ON s.PriceGroupID = pg.PriceGroupID
		LEFT JOIN PricesDL pdl ON s.PriceGroupID = pdl.PriceGroupID
		LEFT JOIN PricesMC pmc ON s.PriceGroupID = pmc.PriceGroupID
	WHERE s.MaterialID = @materialId
		AND s.BranchTypeID = @branchTypeId
		AND pdl.DateEnd IS NULL
		AND pmc.DateEnd IS NULL
)