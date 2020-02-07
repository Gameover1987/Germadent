-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 16.12.2019
-- Description:	Внесение изменений в заказ-наряд ЗТЛ
-- =============================================
CREATE PROCEDURE [dbo].[UpdateWorkOrderDL]
	
	@workOrderID int
	, @status tinyint
	, @docNumber nvarchar(10)
--	, @customerID int
	, @customerName nvarchar(100)
--	, @responsiblePersonId int
	, @dateDelivery datetime
	, @flagWorkAccept bit
	, @prostheticArticul nvarchar(50)
	, @workDescription nvarchar(250)
--	, @officeAdminID int
	, @officeAdminName nvarchar(50)
	, @closed datetime
	, @doctorFullName nvarchar(150)
--	, @patientID int
	, @patientFullName nvarchar(150)
	, @patientAge tinyint
	, @transparenceID int
	, @fittingDate datetime
	, @dateOfCompletion datetime
	, @colorAndFeatures nvarchar(100)
	
AS
BEGIN
	
	UPDATE WorkOrder
	SET Status = @status
		, DocNumber = @docNumber
--		, CustomerID = @customerID
	 	, CustomerName = @customerName
		, PatientFullName = @patientFullName
--		, ResponsiblePersonID = @responsiblePersonId
--		, PatientID = @patientID
		, DateDelivery = @dateDelivery
		, ProstheticArticul = @prostheticArticul
		, WorkDescription = @workDescription
		, FlagWorkAccept = @flagWorkAccept
--		, OfficeAdminID = @officeAdminID
		, OfficeAdminName = @officeAdminName
		, Closed = @closed
	
	WHERE WorkOrderID = @workOrderID


	UPDATE WorkOrderDL
	SET TransparenceID = @transparenceID
		, DoctorFullName = @doctorFullName
		
		, PatientAge = @patientAge
		, FittingDate = @fittingDate
		, DateOfCompletion = @dateOfCompletion
		, ColorAndFeatures = @colorAndFeatures

	WHERE WorkOrderDLID = @workOrderID

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateWorkOrderDL] TO [gdl_user]
    AS [dbo];

