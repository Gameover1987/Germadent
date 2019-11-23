-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 17.11.2019
-- Description:	Выводит на экран список заказ-нарядов по заданным критериям отбора
-- =============================================
CREATE FUNCTION [dbo].[GetWorkOrdersList]
(	
	@branchName nvarchar(30) = NULL, 
	@docNumber nvarchar(10) = NULL,
	@customerName nvarchar(50) = NULL,
	@responsiblePerson nvarchar(100) = NULL,
	@patientFamilyName nchar(30) = NULL,
	@createDateFrom datetime = NULL,
	@createDateTo datetime = NULL,
	@deliveryDateFrom datetime = NULL,
	@deliveryDateTo datetime = NULL
)
RETURNS TABLE 
AS
RETURN 
(	
	SELECT b.BranchName, wo.DocNumber, cs.CustomerName, cs.ResponsiblePerson, p.FamilyName, wo.Created

	FROM WorkOrder wo INNER JOIN Branches b	ON wo.BrachID = b.BranchID
		INNER JOIN Customers cs	ON wo.CustomerID = cs.CustomerID
		INNER JOIN Patients p ON wo.PatientID = p.PatientID

	WHERE b.BranchName = ISNULL(@branchName, b.BranchName)
		AND wo.DocNumber LIKE '%'+ISNULL(@docNumber, '')+'%'
		AND cs.CustomerName LIKE '%'+ISNULL(@customerName, '')+'%'
		AND cs.ResponsiblePerson LIKE '%'+ISNULL(@responsiblePerson, '')+'%'
		AND p.FamilyName LIKE '%'+ISNULL(@patientFamilyName, '')+'%'
		AND wo.Created BETWEEN ISNULL(@createDateFrom, '17530101') AND ISNULL(@createDateTo, '99991231')
		AND wo.DateDelivery BETWEEN ISNULL(@deliveryDateFrom, '17530101') AND ISNULL(@deliveryDateTo, '99991231')
)