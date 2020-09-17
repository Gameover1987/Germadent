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
	SELECT sg.ServiceGroupName, pp.PricePositionCode, pp.PricePositionName, pdl.Price, pmc.PriceSTL, pmc.PriceModel
	FROM ServicesGroups sg 
		INNER JOIN PricePositions pp ON sg.ServiceGroupID = pp.ServiceGroupID
		LEFT JOIN PricesDL pdl ON pp.PricePositionID = pdl.PricePositionID
		LEFT JOIN PricesMC pmc ON pp.PricePositionID = pmc.PricePositionID
	WHERE sg.BranchTypeID = @branchTypeID
		AND (LEN(pdl.DateEnd) = 0 OR LEN(pmc.DateEnd) = 0)
)