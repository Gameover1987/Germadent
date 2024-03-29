﻿-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 17.11.2019
-- Description:	Возвращает список заказ-нарядов по заданным критериям отбора
-- =============================================
CREATE FUNCTION [dbo].[GetWorkOrdersList]
(	
	@branchTypeID int = NULL
	, @branchType nvarchar(30) = NULL
	, @workorderID int = NULL
	, @docNumber nvarchar(10) = NULL
	, @customerName nvarchar(50) = NULL
	, @patientFullName nvarchar(150) = NULL
	, @doctorFullName nvarchar(150) = NULL
	, @createDateFrom datetime = NULL
	, @createDateTo datetime = NULL
	, @userId int = NULL
	, @jsonStringStatus nvarchar(max) = NULL --'[{"StatusName": "Формируется"}]'
	, @materialSet nvarchar(max)
	, @modeller nvarchar(150) = NULL
	, @technician nvarchar(150) = NULL
	, @operator nvarchar(150) = NULL
)
RETURNS TABLE 
AS
RETURN 
(	
	WITH statusChanged (WorkOrderID, StatusChangeDateTime) AS
	(SELECT WorkOrderID, MAX(StatusChangeDateTime) AS StatusChangeDateTime
		FROM dbo.StatusList
		GROUP BY WorkOrderID),

	currentStatus (WorkOrderID, Status, StatusChangeDateTime) AS 
	(SELECT statusChanged.WorkOrderID, sls.Status, statusChanged.StatusChangeDateTime
		FROM dbo.StatusList sls INNER JOIN  statusChanged ON sls.WorkOrderID = statusChanged.WorkOrderID AND sls.StatusChangeDateTime = statusChanged.StatusChangeDateTime),

	-- Находим моделировщиков
	modell AS
	(SELECT DISTINCT wl.WorkOrderID, CONCAT(u.FamilyName,' ', LEFT(u.FirstName, 1), '.', LEFT(u.Patronymic, 1), '.') AS UserFullName
	FROM dbo.WorkList wl INNER JOIN dbo.TechnologyOperations teop on wl.TechnologyOperationID = teop.TechnologyOperationID AND teop.EmployeePositionID = 2
		INNER JOIN dbo.Users u on wl.EmployeeIDStarted = u.UserID
	),
	modellers AS
	(SELECT modell.WorkOrderID, STRING_AGG(modell.UserFullName, '; ') AS UserFullName
		FROM modell
		GROUP BY modell.WorkOrderID
	),

	-- Находим техников
	techn AS
	(SELECT DISTINCT wl.WorkOrderID, CONCAT(u.FamilyName,' ', LEFT(u.FirstName, 1), '.', LEFT(u.Patronymic, 1), '.') AS UserFullName
		FROM dbo.WorkList wl INNER JOIN dbo.TechnologyOperations teop on wl.TechnologyOperationID = teop.TechnologyOperationID AND teop.EmployeePositionID = 3
		INNER JOIN dbo.Users u on wl.EmployeeIDStarted = u.UserID
	),
	technicians AS
	(SELECT techn.WorkOrderID, STRING_AGG(techn.UserFullName, '; ') AS UserFullName
		FROM techn
		GROUP BY techn.WorkOrderID
	),

	-- Находим операторов
	oper AS
	(SELECT DISTINCT wl.WorkOrderID, CONCAT(u.FamilyName,' ', LEFT(u.FirstName, 1), '.', LEFT(u.Patronymic, 1), '.') AS UserFullName
	FROM dbo.WorkList wl INNER JOIN dbo.TechnologyOperations teop on wl.TechnologyOperationID = teop.TechnologyOperationID AND teop.EmployeePositionID = 4
		INNER JOIN dbo.Users u on wl.EmployeeIDStarted = u.UserID
	),
	operators AS
	(SELECT oper.WorkOrderID, STRING_AGG(oper.UserFullName, '; ') AS UserFullName
		FROM oper
		GROUP BY oper.WorkOrderID
	)

	-- Собираем всё вместе
		SELECT b.BranchTypeID
		, b.BranchType
		, wo.WorkOrderID
		, wo.DocNumber	
		, c.CustomerName
		, wo.PatientFullName
		, rp.ResponsiblePerson AS DoctorFullName
		, swozero.UserID AS CreatorID
		, swozero.StatusChangeDateTime as Created
		, se.Status
		, se.StatusName
		, currentStatus.StatusChangeDateTime
		, wo.FlagWorkAccept
		, wo.FlagStl
		, CONCAT(u.FamilyName,' ', LEFT(u.FirstName, 1), '.', LEFT(u.Patronymic, 1), '.') AS CreatorFullName
		, occ.UserID AS LockedBy
		, occ.OccupancyDateTime AS LockDate
		, ISNULL(modellers.UserFullName, '') AS Modeller
		, ISNULL(technicians.UserFullName, '') AS Technician
		, ISNULL(operators.UserFullName, '') AS Operator

	FROM dbo.WorkOrder wo 
		INNER JOIN dbo.BranchTypes b ON wo.BranchTypeID = b.BranchTypeID
		INNER JOIN dbo.Customers c ON wo.CustomerID = c.CustomerID
		INNER JOIN currentStatus ON wo.WorkOrderID = currentStatus.WorkOrderID
		INNER JOIN StatusEnumeration se ON currentStatus.Status = se.Status
		INNER JOIN dbo.StatusList swozero ON wo.WorkOrderID = swozero.WorkOrderID AND swozero.Status = 0
		LEFT JOIN dbo.ResponsiblePersons rp ON wo.ResponsiblePersonID = rp.ResponsiblePersonID
		LEFT JOIN dbo.Users u ON swozero.UserID = u.UserID
		LEFT JOIN dbo.OccupancyWO occ ON wo.WorkOrderID = occ.WorkOrderID
		LEFT JOIN modellers ON wo.WorkOrderID = modellers.WorkOrderID
		LEFT JOIN technicians ON wo.WorkOrderID = technicians.WorkOrderID
		LEFT JOIN operators ON wo.WorkOrderID = operators.WorkOrderID
	
	WHERE b.BranchTypeID = ISNULL(@branchTypeID, b.BranchTypeID)
		AND b.BranchType LIKE '%'+ISNULL(@branchType, '')+'%'
		AND wo.WorkOrderID = ISNULL(@workorderID, wo.WorkOrderID)
		AND wo.DocNumber LIKE '%'+ISNULL(@docNumber, '')+'%'
		AND c.CustomerName LIKE '%'+ISNULL(@customerName, '')+'%'
		AND (wo.PatientFullName LIKE '%'+ISNULL(@patientFullName, '')+'%' OR (PatientFullName IS NULL AND @patientFullName IS NULL))
		AND (rp.ResponsiblePerson LIKE '%'+ISNULL(@doctorFullName, '')+'%' OR (rp.ResponsiblePerson IS NULL AND @doctorFullName IS NULL))
		AND (modellers.UserFullName LIKE '%'+ISNULL(@modeller, '')+'%' OR(modellers.UserFullName IS NULL AND @modeller IS NULL))
		AND (technicians.UserFullName LIKE '%'+ISNULL(@technician, '')+'%' OR(technicians.UserFullName IS NULL AND @technician IS NULL))
		AND (operators.UserFullName LIKE '%'+ISNULL(@operator, '')+'%' OR(operators.UserFullName IS NULL AND @operator IS NULL))
		AND (swozero.StatusChangeDateTime BETWEEN ISNULL(@createDateFrom, '17530101') AND ISNULL(@createDateTo, '99991231'))
		AND (se.Status IN (SELECT StatusNumber FROM OPENJSON(@jsonStringStatus) WITH (StatusNumber int)) OR @jsonStringStatus IS NULL)
		AND (wo.WorkOrderID IN (SELECT DISTINCT WorkOrderID FROM WorkList wl WHERE wl.EmployeeIDStarted = @userId) OR @userId IS NULL)
		AND (wo.WorkOrderID IN (SELECT DISTINCT WorkOrderID FROM dbo.ToothCard WHERE MaterialID IN (SELECT Id FROM OPENJSON(@materialSet) WITH (Id int))) OR @materialSet IS NULL)
)