-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 21.11.2019
-- Description:	Получение заказ-наряда по ID
-- =============================================
CREATE FUNCTION [dbo].[GetWorkOrderByID] 
(	
	-- Add the parameters for the function here
	@workOrderID int
)
RETURNS TABLE 
AS
RETURN 
(
	
	SELECT wo.WorkOrderID, b.BranchName, wo.DocNumber, wo.Status, cs.CustomerName, p.FamilyName,
			wmc.AdditionalInfo, wmc.Carcass, wmc.ImplantSystem, wmc.IndividualAbutmentProcessing,
			e.FamilyName AS Администратор
	FROM WorkOrder wo INNER JOIN WorkOrderMC wmc ON wo.WorkOrderID = wmc.WorkOrderMCID
			INNER JOIN Branches b ON wo.BrachID = b.BranchID
			INNER JOIN Customers cs ON wo.CustomerID = cs.CustomerID
			INNER JOIN Patients p ON wo.PatientID = p.PatientID	
			INNER JOIN Employee e ON wo.AdministratorID = e.EmployeeID

	WHERE wo.WorkOrderID = @workOrderID
		AND b.BranchID = 1
)