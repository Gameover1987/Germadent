-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 29.06.2020
-- Edit date:   03.11.2020
-- Description:	Возвращает актуальный прайс-лист на услуги филиала на текущую дату
-- =============================================
CREATE FUNCTION [dbo].[GetPriceListForBranch] 
(	
	@branchTypeID int = NULL
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT pg.BranchTypeID, pg.PriceGroupID, pg.PriceGroupName, pg.IsObsoleteGroup, pp.PricePositionID, pp.PricePositionCode, pp.PricePositionName, pp.IsObsoletePosition, m.MaterialID, m.MaterialName, prod.ProductID, prod.ProductName, prod.IsObsoleteProduct, p.DateBeginning, p.DateEnd, p.PriceSTL, p.PriceModel
	FROM dbo.PriceGroups pg
		LEFT JOIN dbo.PricePositions pp ON pg.PriceGroupID = pp.PriceGroupID		
		LEFT JOIN dbo.ProductSet ps ON pp.PricePositionID = ps.PricePositionID
		LEFT JOIN dbo.Products prod ON ps.ProductID = prod.ProductID
		LEFT JOIN dbo.MaterialSet ms ON pp.PricePositionID = ms.PricePositionID
		LEFT JOIN dbo.Materials m ON ms.MaterialID = m.MaterialID
		LEFT JOIN dbo.Prices p ON pp.PricePositionID = p.PricePositionID
	WHERE pg.BranchTypeID = ISNULL(@branchTypeId, pg.BranchTypeID)
		AND (pg.IsObsoleteGroup IS NULL OR pg.IsObsoleteGroup = 0)
		AND (pp.IsObsoletePosition IS NULL OR pp.IsObsoletePosition = 0)
		AND (prod.IsObsoleteProduct IS NULL OR prod.IsObsoleteProduct = 0)
		AND GETDATE() BETWEEN ISNULL(p.DateBeginning, '17530101') AND ISNULL(p.DateEnd, '99991231')

)