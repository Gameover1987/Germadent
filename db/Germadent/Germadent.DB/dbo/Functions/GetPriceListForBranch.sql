-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 25.03.2020
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
	SELECT sg.ServiceGroupName, s.SeviceName, pdl.Price, pmc.PriceSTL, pmc.PriceModel
	FROM Servicess s INNER JOIN ServicesGroups sg ON s.ServiceGroupID = sg.ServiceGroupID
		LEFT JOIN PricesDL pdl ON s.ServiceID = pdl.PriceGroupID
		LEFT JOIN PricesMC pmc ON s.ServiceID = pmc.PriceGroupID
	WHERE sg.BranchTypeID = @branchTypeID
		AND (LEN(pdl.DateEnd) = 0 OR LEN(pmc.DateEnd) = 0)
)