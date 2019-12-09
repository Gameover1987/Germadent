-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 17.11.2019
-- Description:	Выводит на экран список заказ-нарядов по заданным критериям отбора
-- =============================================
CREATE FUNCTION [dbo].[GetWorkOrdersList]
(	
	@branchTypeID int = NULL,
	@branchType nvarchar(30) = NULL, 
	@docNumber nvarchar(10) = NULL,
	@customerName nvarchar(50) = NULL,
	@responsiblePerson nvarchar(100) = NULL,
	@patientFamilyName nvarchar(30) = NULL,
	@createDateFrom datetime = NULL,
	@createDateTo datetime = NULL,
	@closeDateFrom datetime = NULL,
	@closeDateTo datetime = NULL
)
RETURNS TABLE 
AS
RETURN 
(	
	SELECT b.BranchTypeID, b.BranchType, wo.DocNumber, cs.CustomerName, cs.ResponsiblePerson, 
			CONCAT(p.FamilyName,' ', LEFT(p.Name, 1), '.', LEFT(p.Patronymic, 1), '.') AS PatientFNP, 
			wo.Created, wo.Status, wo.Closed

	FROM WorkOrder wo INNER JOIN BranchTypes b	ON wo.BranchTypeID = b.BranchTypeID
		INNER JOIN Customers cs	ON wo.CustomerID = cs.CustomerID
		INNER JOIN Patients p ON wo.PatientID = p.PatientID

	WHERE b.BranchTypeID = ISNULL(@branchTypeID, b.BranchTypeID)
		AND b.BranchType = ISNULL(@branchType, b.BranchType)
		AND wo.DocNumber LIKE '%'+ISNULL(@docNumber, '')+'%'
		AND cs.CustomerName LIKE '%'+ISNULL(@customerName, '')+'%'
		AND cs.ResponsiblePerson LIKE '%'+ISNULL(@responsiblePerson, '')+'%'
		AND p.FamilyName LIKE '%'+ISNULL(@patientFamilyName, '')+'%'
		AND (wo.Created BETWEEN ISNULL(@createDateFrom, '17530101') AND ISNULL(@createDateTo, '99991231')
				OR wo.Closed BETWEEN ISNULL(@closeDateFrom, NULL) AND ISNULL(@closeDateTo, NULL)) -- Пока так, поскольку дата завершения заказов - пустая. Дальше надо что-то с этим делать
)