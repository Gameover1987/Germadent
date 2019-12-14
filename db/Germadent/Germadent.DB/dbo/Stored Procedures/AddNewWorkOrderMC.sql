-- =============================================
-- Author:		<Alexey Kolosenok>
-- Create date: <16.11.2019>
-- Description:	<Добавление заказ-нарядов фрезерного центра>
-- =============================================
CREATE PROCEDURE [dbo].[AddNewWorkOrderMC] 
	
	@docNumber nchar(10),
	@customerID int,
	@responsiblePersonId int,
	@patientID int,
	@workDescription nvarchar(250) = NULL,
	@officeAdminID int,
	@additionalInfo nvarchar(70) = NULL,
	@carcassColor nvarchar(30) = NULL,
	@implantSystem nvarchar(70) = NULL,
	@individualAbutmentProcessing nvarchar(70) = NULL,
	@understaff nvarchar(100) = NULL,
	@workOrderID int	 output

AS
BEGIN

	-- Чтобы неоправданно не возрастало значение Id в ключевом поле - сначала его "подбивка":
	
	DECLARE @max_Id int
	SELECT @max_Id = MAX(WorkOrderID)
	FROM WorkOrder
	DBCC checkident (WorkOrder, reseed, @max_Id)

	-- Собственно вставка:
	
	INSERT INTO WorkOrder
		(BranchTypeID, DocNumber, CustomerID, ResponsiblePersonID, PatientID, Created, WorkDescription, OfficeAdminID)
	VALUES 
		(1, @docNumber, @customerID, @responsiblePersonId, @patientID, GETDATE(), @workDescription, @officeAdminID)

	SET @workOrderID = SCOPE_IDENTITY()

	INSERT INTO WorkOrderMC
		(WorkOrderMCID, AdditionalInfo, CarcassColor, ImplantSystem, IndividualAbutmentProcessing, Understaff)
	VALUES
		(@workOrderID, @additionalInfo, @carcassColor, @implantSystem, @individualAbutmentProcessing, @understaff)


END