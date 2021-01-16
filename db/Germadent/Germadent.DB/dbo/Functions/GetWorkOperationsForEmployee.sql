-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 16.01.2021
-- Description:	Возвращает набор доступных технологических операций с расценками для сотрудника в соответствии с его должностью и квалификацией
-- =============================================
CREATE FUNCTION GetWorkOperationsForEmployee
(	
	@userId int
	
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT u.UserID, ep.EmployeePositionID, ep.EmployeePositionName, epc.QualifyingRank, teo.TechnologyOperationID, teo.TechnologyOperationName
	FROM TechnologyOperations teo 
		INNER JOIN EmployeePositions ep ON teo.EmployeePositionID = ep.EmployeePositionID
		INNER JOIN EmployeePositionsCombination epc ON ep.EmployeePositionID = epc.EmployeePositionID
		INNER JOIN Users u ON epc.EmployeeID = u.UserID
		INNER JOIN Rates r ON teo.TechnologyOperationID = r.TechnologyOperationID AND epc.QualifyingRank = r.QualifyingRank
	WHERE u.UserID = @userId
		AND GETDATE() BETWEEN ISNULL(r.DateBeginning, '17530101') AND ISNULL(r.DateEnd, '99991231')	
)