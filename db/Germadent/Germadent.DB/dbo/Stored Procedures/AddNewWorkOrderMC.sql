-- =============================================
-- Author:		<Alexey Kolosenok>
-- Create date: <16.11.2019>
-- Description:	<Добавление заказ-нарядов фрезерного центра>
-- =============================================
CREATE PROCEDURE [dbo].[AddNewWorkOrderMC] 
	
	@docNumber nchar(10),
	@customerID int,
	@patientID int,
	@workDescription nvarchar(250) = NULL,
	@officeAdminID int,
	@additionalInfo nvarchar(70) = NULL,
	@carcassColor nvarchar(30) = NULL,
	@implantSystem nvarchar(70) = NULL,
	@individualAbutmentProcessing nvarchar(70) = NULL,
	@understaff nvarchar(100) = NULL

AS
BEGIN
	
	DECLARE @max_Id int,
			@workOrderID int;

	-- Чтобы неоправданно не возрастало значение Id в ключевом поле - сначала его "подбивка":
	SELECT @max_Id = MAX(WorkOrderID)
	FROM WorkOrder
	DBCC checkident (WorkOrder, reseed, @max_Id)

	-- Собственно вставка:
	
	INSERT INTO WorkOrder
		(BranchTypeID, DocNumber, CustomerID, PatientID, Created, WorkDescription, OfficeAdminID)
	VALUES 
		(1, @docNumber, @customerID, @patientID, GETDATE(), @workDescription, @officeAdminID)

	SELECT @workOrderID = SCOPE_IDENTITY()

	INSERT INTO WorkOrderMC
		(WorkOrderMCID, AdditionalInfo, CarcassColor, ImplantSystem, IndividualAbutmentProcessing, Understaff)
	VALUES
		(@workOrderID, @additionalInfo, @carcassColor, @implantSystem, @individualAbutmentProcessing, @understaff)


END