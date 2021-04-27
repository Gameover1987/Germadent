-- =============================================
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
	, @closeDateFrom datetime = NULL
	, @closeDateTo datetime = NULL
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
		FROM dbo.StatusList sls INNER JOIN  statusChanged ON sls.WorkOrderID = statusChanged.WorkOrderID AND sls.StatusChangeDateTime = statusChanged.StatusChangeDateTime)

		SELECT  b.BranchTypeID
		, b.BranchType
		, wo.WorkOrderID
		, wo.DocNumber	
		, c.CustomerName
		, wo.PatientFullName
		, rp.ResponsiblePerson AS DoctorFullName
		, swozero.StatusChangeDateTime as Created
		, CONCAT(cs.Status, ' - ', se.StatusName) AS Status
		, wo.FlagWorkAccept
		, swoend.StatusChangeDateTime Closed
		, CONCAT(u.FamilyName,' ', LEFT(u.FirstName, 1), '.', LEFT(u.Patronymic, 1), '.') AS CreatorFullName

	FROM dbo.WorkOrder wo 
		INNER JOIN dbo.BranchTypes b ON wo.BranchTypeID = b.BranchTypeID
		INNER JOIN dbo.Customers c ON wo.CustomerID = c.CustomerID
		INNER JOIN currentStatus cs ON wo.WorkOrderID = cs.WorkOrderID
		INNER JOIN StatusEnumeration se ON cs.Status = se.Status
		INNER JOIN dbo.StatusList swozero ON wo.WorkOrderID = swozero.WorkOrderID AND swozero.Status = 0
		LEFT JOIN dbo.StatusList swoend ON wo.WorkOrderID = swoend.WorkOrderID AND swoend.Status = 9
		LEFT JOIN dbo.ResponsiblePersons rp ON wo.ResponsiblePersonID = rp.ResponsiblePersonID
		LEFT JOIN dbo.Users u ON wo.CreatorID = u.UserID
	
	WHERE b.BranchTypeID = ISNULL(@branchTypeID, b.BranchTypeID)
		AND b.BranchType LIKE '%'+ISNULL(@branchType, '')+'%'
		AND wo.WorkOrderID = ISNULL(@workorderID, wo.WorkOrderID)
		AND wo.DocNumber LIKE '%'+ISNULL(@docNumber, '')+'%'
		AND c.CustomerName LIKE '%'+ISNULL(@customerName, '')+'%'
		AND (wo.PatientFullName LIKE '%'+ISNULL(@patientFullName, '')+'%' 
				OR (PatientFullName IS NULL AND @patientFullName IS NULL))
		AND (rp.ResponsiblePerson LIKE '%'+ISNULL(@doctorFullName, '')+'%' 
				OR (rp.ResponsiblePerson IS NULL AND @doctorFullName IS NULL))
		AND (swozero.StatusChangeDateTime BETWEEN ISNULL(@createDateFrom, '17530101') AND ISNULL(@createDateTo, '99991231'))
		AND	(swoend.StatusChangeDateTime BETWEEN ISNULL(@closeDateFrom, '17530101') AND ISNULL(@closeDateTo, '99991231') 
				OR (swoend.StatusChangeDateTime IS NULL AND @closeDateFrom IS NULL AND @closeDateTo IS NULL))
		
)