-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 16.12.2019
-- Description:	Внесение изменений в заказ-наряд ФЦ
-- =============================================
CREATE PROCEDURE [dbo].[UpdateWorkOrderMC] 
	@workOrderID int	
	, @status tinyint
	, @docNumber nvarchar(10)
--	, @customerID int
	, @customerName nvarchar(100)
--	, @responsiblePersonId int
--	, @patientID int
	, @patientFullName nvarchar(150)
	, @dateDelivery datetime
	, @flagWorkAccept bit
	, @dateComment nvarchar(50)
	, @prostheticArticul nvarchar(50)
	, @workDescription nvarchar(250)
--	, @officeAdminID int
	, @officeAdminName nvarchar(50)
	, @technicFullName nvarchar(150)
	, @technicPhone varchar(20)
	, @additionalInfo nvarchar(70)
	, @carcassColor nvarchar(30)
	, @implantSystem nvarchar(70)
	, @individualAbutmentProcessing nvarchar(70)
	, @understaff nvarchar(100)


AS
BEGIN
	
	SET NOCOUNT ON;

	IF((SELECT Status FROM WorkOrder WHERE WorkOrderID = @workOrderID) = 2)
		BEGIN
			RETURN
		END

	UPDATE WorkOrder
	SET Status = @status
		, DocNumber = @docNumber
	--	, CustomerID = @customerID
	 	, CustomerName = @customerName
	--	, ResponsiblePersonID = @responsiblePersonId
	--	, PatientID = @patientID
		, PatientFullName = @patientFullName
		, DateDelivery = @dateDelivery
		, DateComment = @dateComment
		, ProstheticArticul = @prostheticArticul
		, WorkDescription = @workDescription
		, FlagWorkAccept = @flagWorkAccept
	--	, OfficeAdminID = @officeAdminID
		, OfficeAdminName = @officeAdminName
	
	WHERE WorkOrderID = @workOrderID

	UPDATE WorkOrderMC
	SET TechnicFullName = @technicFullName
		, TechnicPhone = @technicPhone
		, AdditionalInfo = @additionalInfo
		, CarcassColor = @carcassColor
		, ImplantSystem = @implantSystem
		, IndividualAbutmentProcessing = @individualAbutmentProcessing
		, Understaff = @understaff

	WHERE WorkOrderMCID = @workOrderID

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateWorkOrderMC] TO [gdl_user]
    AS [dbo];

