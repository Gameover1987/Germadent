﻿-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 21.11.2019
-- Description:	Возвращает заказ-наряд по ID. Если не указан конкретный id - выводим весь список
-- =============================================
CREATE FUNCTION [dbo].[GetWorkOrderById] 
(	
	@workOrderID int = NULL
)
RETURNS TABLE 
AS
RETURN 
(
	
SELECT wo.WorkOrderID, wo.DocNumber, b.BranchTypeID, b.BranchType, 
		/*	cs.CustomerID, cs.CustomerName, p.PatientID,
			CONCAT_WS(' ', p.FamilyName, p.Name, p.Patronymic) AS PatientFullName, 
			ISNULL((SELECT dbo.GetPersonAge(p.Birthday)), 0) AS PatientAge, 
			p.Gender AS PatientGender,
			rp.RP_Position, rp.ResponsiblePerson, rp.RP_Phone, 
		*/
			wo.CustomerName,
			ISNULL(wo.PatientFullName, '') AS PatientFullName,
			ISNULL(wo.Created, '') AS Created,
			wo.Status,
			ISNULL(wo.ProstheticArticul, '') AS ProstheticArticul,
			ISNULL(wo.WorkDescription, '') AS WorkDescription,
			wo.FlagWorkAccept,
			ISNULL(wo.OfficeAdminName, '') AS OfficeAdminName,
			wo.Closed,
		--	CONCAT(e.FamilyName,' ', LEFT(e.Name, 1), '.', LEFT(e.Patronymic, 1), '.') AS OfficeAdmin,
			ISNULL(wmc.TechnicFullName, '') AS TechnicFullName,
			ISNULL(wmc.TechnicPhone, '') AS TechnicPhone,
			ISNULL(wmc.AdditionalInfo, '') AS AdditionalInfo,
			ISNULL(wmc.CarcassColor, '') AS CarcassColor,
			ISNULL(wmc.ImplantSystem, '') AS ImplantSystem,
			ISNULL(wmc.IndividualAbutmentProcessing, '') AS IndividualAbutmentProcessing,
			ISNULL(wmc.Understaff, '') AS Understaff,
			ISNULL(wdl.DoctorFullName, '') AS DoctorFullName,			
			ISNULL(wdl.PatientAge, 0) AS PatientAge,
			wdl.DateOfCompletion,
			wdl.FittingDate,
			ISNULL(wdl.ColorAndFeatures, '') AS ColorAndFeatures,
			ISNULL(wdl.TransparenceID, 0) AS TransparenceID,
	--		ISNULL(tr.TransparenceName, '') AS TransparenceName
			dbo.GetMaterialsEnumByWOId(wo.WorkOrderID) AS MaterialsEnum

	FROM	 WorkOrder wo 
			INNER JOIN BranchTypes b ON wo.BranchTypeID = b.BranchTypeID
	--		INNER JOIN Customers cs ON wo.CustomerID = cs.CustomerID
	--		INNER JOIN ResponsiblePersons rp ON wo.ResponsiblePersonID = rp.ResponsiblePersonID
	--		INNER JOIN Patients p ON wo.PatientID = p.PatientID	
	--		INNER JOIN Employee e ON wo.OfficeAdminID = e.EmployeeID
			LEFT JOIN WorkOrderMC wmc ON wo.WorkOrderID = wmc.WorkOrderMCID
			LEFT JOIN WorkOrderDL wdl ON wo.WorkOrderID = wdl.WorkOrderDLID
	--		LEFT JOIN Transparences tr ON wdl.TransparenceID = tr.TransparenceID

	WHERE wo.WorkOrderID = ISNULL(@workOrderID, wo.WorkOrderID)
)