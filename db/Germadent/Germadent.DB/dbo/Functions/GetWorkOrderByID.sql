-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 21.11.2019
-- Description:	Получение заказ-наряда по ID. Если не указан конкретный id - выводим весь список
-- =============================================
CREATE FUNCTION [dbo].[GetWorkOrderById] 
(	
	@workOrderID int = NULL
)
RETURNS TABLE 
AS
RETURN 
(
	
	SELECT wo.WorkOrderID, wo.DocNumber, b.BranchTypeID, b.BranchType, cs.CustomerID, cs.CustomerName, 
			p.PatientID,
			CONCAT_WS(' ', p.FamilyName, p.Name, p.Patronymic) AS PatientFNP, 
			ISNULL((SELECT dbo.GetPersonAge(p.Birthday)), 0) AS PatientAge, 
			p.Gender AS PatientGender,
			rp.RP_Position, rp.ResponsiblePersonName, rp.RP_Phone, 
			ISNULL(wo.Created, '') AS Created,
			ISNULL(wo.WorkDescription, '') AS WorkDescription,
			wo.FlagWorkAccept, 
			ISNULL(wo.Closed, '') AS Closed,
			CONCAT(e.FamilyName,' ', LEFT(e.Name, 1), '.', LEFT(e.Patronymic, 1), '.') AS OfficeAdminFNP,
			ISNULL(wmc.AdditionalInfo, '') AS AdditionalInfo,
			ISNULL(wmc.CarcassColor, '') AS CarcassColor,
			ISNULL(wmc.ImplantSystem, '') AS ImplantSystem,
			ISNULL(wmc.IndividualAbutmentProcessing, '') AS IndividualAbutmentProcessing,
			ISNULL(wmc.Understaff, '') AS Understaff,
			ISNULL(wdl.TypeOfWork, '') AS TypeOfWork, 
			ISNULL(wdl.DateOfCompletion, '') DateOfCompletion, 
			ISNULL(wdl.FittingDate, '') AS FittingDate, 
			ISNULL(wdl.ColorAndFeatures, '') AS ColorAndFeatures,
			ISNULL(tr.TransparenceName, '') AS TransparenceName

	FROM	 WorkOrder wo 
			INNER JOIN BranchTypes b ON wo.BranchTypeID = b.BranchTypeID
			INNER JOIN Customers cs ON wo.CustomerID = cs.CustomerID
			INNER JOIN ResponsiblePersons rp ON cs.CustomerID = rp.CustomerID
			INNER JOIN Patients p ON wo.PatientID = p.PatientID	
			INNER JOIN Employee e ON wo.OfficeAdminID = e.EmployeeID
			LEFT JOIN WorkOrderMC wmc ON wo.WorkOrderID = wmc.WorkOrderMCID
			LEFT JOIN WorkOrderDL wdl ON wo.WorkOrderID = wdl.WorkOrderDLID
			LEFT JOIN Transparences tr ON wdl.TransparenceID = tr.TransparenceID

	WHERE wo.WorkOrderID = ISNULL(@workOrderID, wo.WorkOrderID)
)