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
	, @dateDelivery datetime
	, @flagWorkAccept bit
	, @workDescription nvarchar(250)
--	, @officeAdminID int
	, @officeAdminName nvarchar(50)
	, @closed datetime
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

	UPDATE WorkOrder
	SET Status = @status
		, DocNumber = @docNumber
	--	, CustomerID = @customerID
	 	, CustomerName = @customerName
	--	, ResponsiblePersonID = @responsiblePersonId
	--	, PatientID = @patientID
		, DateDelivery = @dateDelivery
		, WorkDescription = @workDescription
		, FlagWorkAccept = @flagWorkAccept
	--	, OfficeAdminID = @officeAdminID
		, OfficeAdminName = @officeAdminName
		, Closed = @closed
	
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