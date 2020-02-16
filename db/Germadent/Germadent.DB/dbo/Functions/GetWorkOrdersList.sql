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
		, wo.PatientFullName
		, CASE 
				WHEN b.BranchTypeID = 2 THEN wdl.DoctorFullName
				WHEN b.BranchTypeID = 1 THEN wmc.TechnicFullName
				END AS DoctorFullName
		, wo.Created 
		, wo.Status
		, wo.FlagWorkAccept
		, wo.Closed
		, wo.WorkDescription

	FROM WorkOrder wo INNER JOIN BranchTypes b ON wo.BranchTypeID = b.BranchTypeID
		LEFT JOIN WorkOrderDL wdl ON wo.WorkOrderID = wdl.WorkOrderDLID
		LEFT JOIN WorkOrderMC wmc ON wo.WorkOrderID = wmc.WorkOrderMCID

	
	WHERE b.BranchTypeID = ISNULL(@branchTypeID, b.BranchTypeID)
		AND b.BranchType LIKE '%'+ISNULL(@branchType, '')+'%'
		AND wo.WorkOrderID = ISNULL(@workorderID, wo.WorkOrderID)
		AND wo.DocNumber LIKE '%'+ISNULL(@docNumber, '')+'%'
		AND wo.CustomerName LIKE '%'+ISNULL(@customerName, '')+'%'
		AND (wo.PatientFullName LIKE '%'+ISNULL(@patientFullName, '')+'%' 
				OR (PatientFullName IS NULL AND @patientFullName IS NULL))
		AND (wdl.DoctorFullName LIKE '%'+ISNULL(@doctorFullName, '')+'%' 
				OR wmc.TechnicFullName LIKE '%'+ISNULL(@doctorFullName, '')+'%' 
				OR (wdl.DoctorFullName IS NULL AND @doctorFullName IS NULL) 
				OR (wmc.TechnicFullName IS NULL AND @doctorFullName IS NULL))
		AND (wo.Created BETWEEN ISNULL(@createDateFrom, '17530101') AND ISNULL(@createDateTo, '99991231'))
		AND	(wo.Closed BETWEEN ISNULL(@closeDateFrom, '17530101') AND ISNULL(@closeDateTo, '99991231') 
				OR (Closed IS NULL AND @closeDateFrom IS NULL AND @closeDateTo IS NULL))
		
)