-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 29.06.2020
-- Edit date:   17.09.2020
-- Description:	Возвращает прайс-лист на услуги филиала на текущую дату
-- =============================================
CREATE FUNCTION [dbo].[GetPriceListForBranch] 
(	
	@branchTypeID int
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT pg.PriceGroupID, pg.PriceGroupName, pp.PricePositionID, pp.PricePositionCode, pp.PricePositionName, m.MaterialID, m.MaterialName, pdl.Price, pmc.PriceSTL, pmc.PriceModel
	FROM PriceGroups pg
		INNER JOIN PricePositions pp ON pg.PriceGroupID = pp.PriceGroupID
		INNER JOIN Materials m ON pp.MaterialID = m.MaterialID
		LEFT JOIN PricesDL pdl ON pp.PricePositionID = pdl.PricePositionID
		LEFT JOIN PricesMC pmc ON pp.PricePositionID = pmc.PricePositionID
	WHERE pg.BranchTypeID = @branchTypeId
		AND (pdl.DateEnd IS NULL OR pmc.DateEnd IS NULL)
)