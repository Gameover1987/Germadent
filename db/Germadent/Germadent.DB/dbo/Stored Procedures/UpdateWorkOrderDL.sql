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
	, @dateComment nvarchar(50)
	, @prostheticArticul nvarchar(50)
	, @workDescription nvarchar(250)
--	, @officeAdminID int
	, @officeAdminName nvarchar(50)
	, @doctorFullName nvarchar(150)
--	, @patientID int
	, @patientFullName nvarchar(150)
	, @patientGender bit
	, @patientAge tinyint
	, @transparenceID int
	, @fittingDate datetime
	, @dateOfCompletion datetime
	, @colorAndFeatures nvarchar(100)
	
AS
BEGIN
	
	IF((SELECT Status FROM WorkOrder WHERE WorkOrderID = @workOrderID) = 9)
		BEGIN
			RETURN
		END

	UPDATE WorkOrder
	SET Status = @status
		, DocNumber = @docNumber
--		, CustomerID = @customerID
	 	, CustomerName = @customerName
		, PatientFullName = @patientFullName
--		, ResponsiblePersonID = @responsiblePersonId
--		, PatientID = @patientID
		, DateDelivery = @dateDelivery
		, DateComment = @dateComment
		, ProstheticArticul = @prostheticArticul
		, WorkDescription = @workDescription
		, FlagWorkAccept = @flagWorkAccept
--		, OfficeAdminID = @officeAdminID
		, OfficeAdminName = @officeAdminName
	
	WHERE WorkOrderID = @workOrderID


	UPDATE WorkOrderDL
	SET TransparenceID = @transparenceID
		, DoctorFullName = @doctorFullName
		, PatientGender = @patientGender
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

