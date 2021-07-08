-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 06.07.2021
-- Description:	Формирует уточнённые критерии и вызывает функцию возврата списка заказ-нарядов по базовым критериям отбора и в зависимости от должности сотрудника
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
	, @materialSet nvarchar(max) = NULL
	, @showTheirWO bit = 0
)

AS
BEGIN 
	
	SET NOCOUNT ON;

	DECLARE @employeePositionId int
				
	CREATE TABLE #positionsId (EmployeePositionId int)
	-- Во временную таблицу закидываем набор должностей для сотрудинка
	INSERT #positionsId
	SELECT EmployeePositionID
	FROM dbo.EmployeePositionsCombination
	WHERE EmployeeID = @userId

	
	IF (SELECT COUNT(WorkOrderID) FROM dbo.GetWorkOrderIdForMaterialSelect(@materialSet)) = 0 
		SET @materialSet = NULL

	-- Показывать весь список з-н, а не только со своими работами
	IF @showTheirWO = 0
		SET @userId = NULL		

	-- Если нет совмещения должностей - смотрим, кому какой список заказ-нарядов показывать
	IF (SELECT COUNT(1) FROM #positionsId) = 1 BEGIN

		SELECT @employeePositionId = EmployeePositionId FROM #positionsId

		IF @employeePositionId = 4 -- оператор
		SELECT * 
		FROM dbo.GetWorkOrdersList
		(@branchTypeID, @branchType, @workorderID, @docNumber, @customerName	, @patientFullName, @doctorFullName, @createDateFrom	, @createDateTo, @userId	, @jsonStringStatus)
		WHERE (Status BETWEEN 9 AND 99 OR (BranchTypeID = 1 AND FlagStl = 1 AND Status = 0))
			AND (WorkOrderID IN (SELECT * FROM dbo.GetWorkOrderIdForMaterialSelect(@materialSet)) OR @materialSet IS NULL) -- перечень материалов из фильтра, если таковой есть

		IF @employeePositionId = 2 -- моделировщик
		SELECT * 
		FROM dbo.GetWorkOrdersList
		(@branchTypeID, @branchType, @workorderID, @docNumber, @customerName	, @patientFullName, @doctorFullName, @createDateFrom	, @createDateTo, @userId	, @jsonStringStatus)
		WHERE (BranchTypeID = 1 AND FlagStl = 0)
			AND (WorkOrderID IN (SELECT * FROM dbo.GetWorkOrderIdForMaterialSelect(@materialSet)) OR @materialSet IS NULL)

		IF @employeePositionId = 3 -- техник
		SELECT * 
		FROM dbo.GetWorkOrdersList
		(@branchTypeID, @branchType, @workorderID, @docNumber, @customerName	, @patientFullName, @doctorFullName, @createDateFrom	, @createDateTo, @userId	, @jsonStringStatus)
		WHERE WorkOrderID IN (SELECT * FROM dbo.GetWorkOrderIdForMaterialSelect(@materialSet)) OR @materialSet IS NULL
		
		END

	ELSE SELECT * 
		FROM dbo.GetWorkOrdersList
		(@branchTypeID, @branchType, @workorderID, @docNumber, @customerName	, @patientFullName, @doctorFullName, @createDateFrom	, @createDateTo, @userId	, @jsonStringStatus)
		WHERE WorkOrderID IN (SELECT * FROM dbo.GetWorkOrderIdForMaterialSelect(@materialSet)) OR @materialSet IS NULL

	DROP TABLE #positionsId
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRelevantWorkOrdersList] TO [gdl_user]
    AS [dbo];

