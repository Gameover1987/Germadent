-- =============================================
-- Author:		Алесей Колосенок
-- Create date: 21.09.2020
-- Description:	Возвращает перечень групп услуг и ценовые позиции для типа филиала
-- =============================================
CREATE FUNCTION GetServiceGroupsAndPricePositions 
(	
	
	@branchTypeId int
	
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT sg.*, pp.PricePositionID, pp.PricePositionCode, pp.PricePositionName
	FROM ServicesGroups sg
		INNER JOIN PricePositions pp ON sg.ServiceGroupID = pp.ServiceGroupID
	WHERE sg.BranchTypeID = @branchTypeId
)