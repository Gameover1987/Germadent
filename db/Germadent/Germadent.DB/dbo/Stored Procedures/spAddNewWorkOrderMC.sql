-- =============================================
-- Author:		<Alexey Kolosenok>
-- Create date: <16.11.2019>
-- Description:	<Добавление заказ-нарядов фрезерного центра>
-- =============================================
CREATE PROCEDURE [dbo].[spAddNewWorkOrderMC] 
	
	@status tinyint,
	@docNumber nchar(10),
	@customerID int,
	@patientID int,
	@workDescription nvarchar(250),
	@additionalInfo nvarchar(70),
	@implantSystem nvarchar(70),
	@individualAbutmentProcessing nvarchar(70)

AS
BEGIN
	
	DECLARE @max_Id int,
			@workOrderID int;

	-- Чтобы неоправданно не возрастало значение Id в ключевом поле - сначала его "подбивка":
	SELECT @max_Id = MAX(WorkOrderID)
	FROM WorkOrder
	DBCC checkident (WorkOrder, reseed, @max_Id);

	-- Собственно вставка:
	
	INSERT INTO WorkOrder
		(BrachID, Status, DocNumber, CustomerID, PatientID, DateTimeCreated, WorkDescription)
	VALUES
		(1,	@status, @docNumber, @customerID, @patientID, GETDATE(), @workDescription)

	SELECT @workOrderID = SCOPE_IDENTITY()

	INSERT INTO WorkOrderMC
		(WorkOrderMCID, AdditionalInfo, ImplantSystem, IndividualAbutmentProcessing)
	VALUES
		(@workOrderID, @additionalInfo, @implantSystem, @individualAbutmentProcessing)
END