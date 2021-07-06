-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 06.07.2021
-- Description:	Возвращает список заказ-нарядов по заданным критериям отбора в зависимости от должности сотрудника
-- =============================================
CREATE PROCEDURE [dbo].[GetRelevantWorkOrdersList]
(	
	@branchTypeID int = NULL
	, @branchType nvarchar(30) = NULL
	, @workorderID int = NULL
	, @docNumber nvarchar(10) = NULL
	, @customerName nvarchar(70) = NULL
	, @patientFullName nvarchar(150) = NULL
	, @doctorFullName nvarchar(150) = NULL
	, @createDateFrom datetime = NULL
	, @createDateTo datetime = NULL
	, @userId int = NULL	
	, @jsonStringStatus nvarchar(max) = NULL
)

AS
BEGIN 
	
	SET NOCOUNT ON;

	DECLARE @employeePositionId int
			
	
	CREATE TABLE #positionsId (EmployeePositionId int)

	INSERT #positionsId
	SELECT EmployeePositionID
	FROM dbo.EmployeePositionsCombination
	WHERE EmployeeID = @userId

	IF (SELECT COUNT(1) FROM #positionsId) = 1 BEGIN

		SELECT @employeePositionId = EmployeePositionId FROM #positionsId

		IF @employeePositionId = 4
		SELECT * 
		FROM dbo.GetWorkOrdersList
		(@branchTypeID, @branchType, @workorderID, @docNumber, @customerName	, @patientFullName, @doctorFullName, @createDateFrom	, @createDateTo, default	, @jsonStringStatus)
		WHERE Status BETWEEN 9 AND 99
			OR (BranchTypeID = 1 AND FlagStl = 1 AND Status = 0)

		IF @employeePositionId = 2
		SELECT * 
		FROM dbo.GetWorkOrdersList
		(@branchTypeID, @branchType, @workorderID, @docNumber, @customerName	, @patientFullName, @doctorFullName, @createDateFrom	, @createDateTo, default	, @jsonStringStatus)
		WHERE BranchTypeID = 1 
			AND FlagStl = 0

		IF @employeePositionId = 3
		SELECT * 
		FROM dbo.GetWorkOrdersList
		(@branchTypeID, @branchType, @workorderID, @docNumber, @customerName	, @patientFullName, @doctorFullName, @createDateFrom	, @createDateTo, default	, @jsonStringStatus)
		
		END

	ELSE SELECT * 
		FROM dbo.GetWorkOrdersList
		(@branchTypeID, @branchType, @workorderID, @docNumber, @customerName	, @patientFullName, @doctorFullName, @createDateFrom	, @createDateTo, default	, @jsonStringStatus)

	DROP TABLE #positionsId
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRelevantWorkOrdersList] TO [gdl_user]
    AS [dbo];

