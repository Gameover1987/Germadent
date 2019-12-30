-- =============================================
-- Author:		<Alexey Kolosenok>
-- Create date: <16.11.2019>
-- Description:	<Добавление заказ-нарядов фрезерного центра>
-- =============================================
CREATE PROCEDURE [dbo].[AddNewWorkOrderMC] 
	
	@customerID int = NULL,
	@customerName nvarchar(100),
	@responsiblePersonId int = NULL,
	@technicFullName nvarchar(150) = NULL,
	@technicPhone varchar(15) = NULL,
	@patientID int = NULL,
	@workDescription nvarchar(250) = NULL,
	@officeAdminID int = NULL,
	@officeAdminName nvarchar(50) = NULL,
	@additionalInfo nvarchar(70) = NULL,
	@carcassColor nvarchar(30) = NULL,
	@implantSystem nvarchar(70) = NULL,
	@individualAbutmentProcessing nvarchar(70) = NULL,
	@understaff nvarchar(100) = NULL,
	@workOrderID int output,
	@docNumber nvarchar(10) output

AS
BEGIN

	-- Чтобы неоправданно не возрастало значение Id в ключевом поле - сначала его "подбивка":	
	DECLARE @max_Id int
	SELECT @max_Id = MAX(WorkOrderID)
	FROM WorkOrder
	DBCC checkident (WorkOrder, reseed, @max_Id)

	-- Генерируем номер документа по принципу сквозной нумерации для обоих филиалов:
	SET @docNumber = CONCAT(CAST((NEXT VALUE FOR dbo.SequenceDocumentNumber) AS nvarchar(6)), '-ФЦ')

	-- Собственно вставка:	
	INSERT INTO WorkOrder
		(BranchTypeID, DocNumber, CustomerID, CustomerName, ResponsiblePersonID, PatientID, Created, WorkDescription, OfficeAdminID, OfficeAdminName)
	VALUES 
		(1, @docNumber, @customerID, @customerName, @responsiblePersonId, @patientID, GETDATE(), @workDescription, @officeAdminID, @officeAdminName)

	SET @workOrderID = SCOPE_IDENTITY()

	INSERT INTO WorkOrderMC
		(WorkOrderMCID, TechnicFullName, TechnicPhone, AdditionalInfo, CarcassColor, ImplantSystem, IndividualAbutmentProcessing, Understaff)
	VALUES
		(@workOrderID, @technicFullName, @technicPhone, @additionalInfo, @carcassColor, @implantSystem, @individualAbutmentProcessing, @understaff)


END