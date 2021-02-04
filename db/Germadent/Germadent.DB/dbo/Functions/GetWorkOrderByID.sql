-- =============================================
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
	
SELECT wo.WorkOrderID, 
			wo.DocNumber,
			b.BranchTypeID,
			b.BranchType, 
			cs.CustomerID,
			cs.CustomerName,	
			ISNULL(rp.ResponsiblePersonID, 0) AS ResponsiblePersonId,
			ISNULL(rp.RP_Position, '') AS RP_Position, 
			ISNULL(rp.ResponsiblePerson, '') AS ResponsiblePerson,
			ISNULL(rp.RP_Phone, '') AS RP_Phone,
			ISNULL(wo.PatientFullName, '') AS PatientFullName,
			ISNULL(wo.Created, '') AS Created,
			wo.Status,
			ISNULL(wo.DateComment, '') AS DateComment,
			ISNULL(wo.ProstheticArticul, '') AS ProstheticArticul,
			ISNULL(wo.WorkDescription, '') AS WorkDescription,
			wo.FlagWorkAccept,
			wo.FlagStl,
			wo.FlagCashless,
			wo.Closed,
			CONCAT(u.FamilyName,' ', LEFT(u.FirstName, 1), '.', LEFT(u.Patronymic, 1), '.') AS CreatorFullName,
			ISNULL(rp.ResponsiblePerson, '') AS TechnicFullName,
			ISNULL(rp.RP_Phone, '') AS TechnicPhone,
			ISNULL(rp.ResponsiblePerson, '') AS DoctorFullName,
			wo.PatientGender,
			ISNULL(wo.PatientAge, 0) AS PatientAge,
			wo.DateOfCompletion,
			wo.FittingDate,
			dbo.GetMaterialsEnumByWOId(wo.WorkOrderID) AS MaterialsEnum

	FROM 	dbo.WorkOrder wo 
			INNER JOIN dbo.BranchTypes b ON wo.BranchTypeID = b.BranchTypeID
			INNER JOIN dbo.Customers cs ON wo.CustomerID = cs.CustomerID
			LEFT JOIN dbo.ResponsiblePersons rp ON wo.ResponsiblePersonID = rp.ResponsiblePersonID
			LEFT JOIN dbo.Users u ON wo.CreatorID = u.UserID

	WHERE wo.WorkOrderID = ISNULL(@workOrderID, wo.WorkOrderID)
)