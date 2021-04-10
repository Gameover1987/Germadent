-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 10.04.2021
-- Description:	Возвращает набор технологических операций, доступных сотруднику согласно его должности
-- =============================================
CREATE FUNCTION dbo.GetRelevantOperationsForPositions
(	
	@userId int	
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT t.TechnologyOperationID, t.TechnologyOperationName
	FROM dbo.Users u 
		INNER JOIN dbo.EmployeePositionsCombination epc ON u.UserID = epc.EmployeeID
		INNER JOIN dbo.EmployeePositions ep ON epc.EmployeePositionID = ep.EmployeePositionID
		INNER JOIN dbo.TechnologyOperations t ON ep.EmployeePositionID = t.EmployeePositionID
)