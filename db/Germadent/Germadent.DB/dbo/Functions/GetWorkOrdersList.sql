-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 17.11.2019
-- Description:	Выводит на экран список заказ-нарядов по заданным критериям отбора
-- =============================================
CREATE FUNCTION [dbo].[GetWorkOrdersList]
(	
	@branchTypeID int = NULL
	, @branchType nvarchar(30) = NULL
	, @workorderID int = NULL
	, @docNumber nvarchar(10) = NULL
	, @customerName nvarchar(50) = NULL
	, @patientFullName nvarchar(30) = NULL
	, @createDateFrom datetime = NULL
	, @createDateTo datetime = NULL
	, @closeDateFrom datetime = NULL
	, @closeDateTo datetime = NULL
)
RETURNS TABLE 
AS
RETURN 
(	
	SELECT  b.BranchTypeID
		, b.BranchType
		, wo.WorkOrderID
		, wo.DocNumber	
		, wo.CustomerName
		, wdl.PatientFullName
		, wo.Created 
		, wo.Status
		, wo.FlagWorkAccept
		, wo.Closed
		, wo.WorkDescription

	FROM WorkOrder wo INNER JOIN BranchTypes b ON wo.BranchTypeID = b.BranchTypeID
		LEFT JOIN WorkOrderDL wdl ON wo.WorkOrderID = wdl.WorkOrderDLID

	
	WHERE b.BranchTypeID = ISNULL(@branchTypeID, b.BranchTypeID)
		AND wo.BranchTypeID LIKE '%'+ISNULL(@branchType, '')+'%'
		AND wo.WorkOrderID = ISNULL(@workorderID, wo.WorkOrderID)
		AND wo.DocNumber LIKE '%'+ISNULL(@docNumber, '')+'%'
		AND wo.CustomerName LIKE '%'+ISNULL(@customerName, '')+'%'
		AND (wdl.PatientFullName LIKE '%'+ISNULL(@patientFullName, '')+'%' OR (PatientFullName IS NULL AND @patientFullName IS NULL))
		AND (wo.Created BETWEEN ISNULL(@createDateFrom, '17530101') AND ISNULL(@createDateTo, '99991231')
				OR wo.Closed BETWEEN ISNULL(@closeDateFrom, NULL) AND ISNULL(@closeDateTo, NULL))
)