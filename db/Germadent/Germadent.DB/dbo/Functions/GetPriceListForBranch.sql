-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 29.06.2020
-- Edit date:   03.11.2020
-- Description:	Возвращает актуальный прайс-лист на услуги филиала на текущую дату
-- =============================================
CREATE FUNCTION [dbo].[GetPriceListForBranch] 
(	
	@branchTypeID int
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT pg.PriceGroupID, pg.PriceGroupName, pp.PricePositionID, pp.PricePositionCode, pp.PricePositionName, m.MaterialID, m.MaterialName, p.PriceSTL, p.PriceModel
	FROM PriceGroups pg
		LEFT JOIN PricePositions pp ON pg.PriceGroupID = pp.PriceGroupID
		LEFT JOIN Materials m ON pp.MaterialID = m.MaterialID
		LEFT JOIN Prices p ON pp.PricePositionID = p.PricePositionID
	WHERE pg.BranchTypeID = @branchTypeId
		AND GETDATE() BETWEEN p.DateBegin AND ISNULL(p.DateEnd, '99991231')

)