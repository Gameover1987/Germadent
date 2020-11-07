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
	SELECT pg.PriceGroupID, pg.PriceGroupName, pp.PricePositionID, pp.PricePositionCode, pp.PricePositionName, m.MaterialID, m.MaterialName, prod.ProstheticsID, prod.ProstheticsName, p.DateBeginning, p.DateEnd, p.PriceSTL, p.PriceModel
	FROM PriceGroups pg
		LEFT JOIN PricePositions pp ON pg.PriceGroupID = pp.PriceGroupID
		LEFT JOIN Materials m ON pp.MaterialID = m.MaterialID
		LEFT JOIN ProductSet ps ON pp.PricePositionID = ps.PricePositionID
		LEFT JOIN TypesOfProsthetics prod ON ps.ProductID = prod.ProstheticsID
		LEFT JOIN Prices p ON pp.PricePositionID = p.PricePositionID
	WHERE pg.BranchTypeID = @branchTypeId
		AND GETDATE() BETWEEN ISNULL(p.DateBeginning, '17530101') AND ISNULL(p.DateEnd, '99991231')

)