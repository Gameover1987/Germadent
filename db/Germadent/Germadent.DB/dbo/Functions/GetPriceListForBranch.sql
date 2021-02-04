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
	SELECT pg.BranchTypeID, pg.PriceGroupID, pg.PriceGroupName, pp.PricePositionID, pp.PricePositionCode, pp.PricePositionName, m.MaterialID, m.MaterialName, prod.ProductID AS ProductID, prod.ProductName AS ProductName, p.DateBeginning, p.DateEnd, p.PriceSTL, p.PriceModel
	FROM dbo.PriceGroups pg
		LEFT JOIN dbo.PricePositions pp ON pg.PriceGroupID = pp.PriceGroupID
		LEFT JOIN dbo.Materials m ON pp.MaterialID = m.MaterialID
		LEFT JOIN dbo.ProductSet ps ON pp.PricePositionID = ps.PricePositionID
		LEFT JOIN dbo.Products prod ON ps.ProductID = prod.ProductID
		LEFT JOIN dbo.Prices p ON pp.PricePositionID = p.PricePositionID
	WHERE pg.BranchTypeID = ISNULL(@branchTypeId, pg.BranchTypeID)
		AND GETDATE() BETWEEN ISNULL(p.DateBeginning, '17530101') AND ISNULL(p.DateEnd, '99991231')

)