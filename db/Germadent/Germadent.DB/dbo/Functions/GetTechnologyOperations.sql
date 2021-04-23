-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 21.04.2021
-- Description:	Возвращает список технологических операций с актуальными расценками, в т.ч. для сотрудника
-- =============================================
CREATE FUNCTION [dbo].[GetTechnologyOperations] 
(	
	@employeeId int = null
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT ep.EmployeePositionID, ep.EmployeePositionName, teo.TechnologyOperationID, teo.TechnologyOperationUserCode, teo.TechnologyOperationName, r.QualifyingRank, r.Rate, r.DateBeginning, r.DateEnd
	FROM dbo.EmployeePositions ep
		INNER JOIN dbo.TechnologyOperations teo ON ep.EmployeePositionID = teo.EmployeePositionID
		LEFT JOIN dbo.Rates r ON teo.TechnologyOperationID = r.TechnologyOperationID
	WHERE GETDATE() BETWEEN ISNULL(r.DateBeginning, '17530101') AND ISNULL(r.DateEnd, '99991231')
		AND ep.EmployeePositionID = ISNULL(@employeeId, ep.EmployeePositionID)
)