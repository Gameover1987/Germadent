-- =============================================
-- Author:		<Alexey Kolosenok>
-- Create date: <16.11.2019>
-- Description:	<Добавление заказ-нарядов фрезерного центра>
-- =============================================
CREATE PROCEDURE [dbo].[spAddNewWorkOrderMC] 
	
	@docNumber nchar(10),
	@customerID int,
	@patientID int,
	@workDescription nvarchar(250) = NULL,
	@administratorID int,
	@additionalInfo nvarchar(70) = NULL,
	@implantSystem nvarchar(70) = NULL,
	@individualAbutmentProcessing nvarchar(70) = NULL

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
		(BrachID, DocNumber, CustomerID, PatientID, Created, WorkDescription, AdministratorID)
	VALUES 
		(1, @docNumber, @customerID, @patientID, GETDATE(), @workDescription, @administratorID)

	SELECT @workOrderID = SCOPE_IDENTITY()

	INSERT INTO WorkOrderMC
		(WorkOrderMCID, AdditionalInfo, ImplantSystem, IndividualAbutmentProcessing)
	VALUES
		(@workOrderID, @additionalInfo, @implantSystem, @individualAbutmentProcessing)


END