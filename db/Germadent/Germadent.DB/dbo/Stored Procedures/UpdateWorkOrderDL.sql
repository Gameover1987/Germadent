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
--		, ResponsiblePersonID = @responsiblePersonId
--		, PatientID = @patientID
		, DateDelivery = @dateDelivery
		, WorkDescription = @workDescription
		, FlagWorkAccept = @flagWorkAccept
--		, OfficeAdminID = @officeAdminID
		, OfficeAdminName = @officeAdminName
		, Closed = @closed
	
	WHERE WorkOrderID = @workOrderID


	UPDATE WorkOrderDL
	SET TransparenceID = @transparenceID
		, DoctorFullName = @doctorFullName
		, PatientFullName = @patientFullName
		, PatientAge = @patientAge
		, FittingDate = @fittingDate
		, DateOfCompletion = @dateOfCompletion
		, ColorAndFeatures = @colorAndFeatures

	WHERE WorkOrderDLID = @workOrderID

END