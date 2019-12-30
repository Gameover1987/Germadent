-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 30.11.2019
-- Description:	Создание заказ-наряда зуботехнической лаборатории
-- =============================================
CREATE PROCEDURE [dbo].[AddNewWorkOrderDL] 
	
	@customerID int = NULL,
	@customerName nvarchar(100),
	@responsiblePersonId int = NULL,
	@doctorFullName nvarchar(150) = NULL,
	@patientID int = NULL,
	@patientFullName nvarchar(150) = NULL,
	@patientAge tinyint = NULL,
	@workDescription nvarchar(250) = NULL,
	@officeAdminID int = NULL,
	@officeAdminName nvarchar(50) = NULL,
	@transparenceID int = NULL,
	@fittingDate datetime = NULL,
	@dateOfCompletion datetime = NULL,
	@colorAndFeatures nvarchar(100) = NULL,
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
	SET @docNumber = CONCAT(CAST((NEXT VALUE FOR dbo.SequenceDocumentNumber) AS nvarchar(6)), '-ЗТЛ')
	
	-- Собственно вставка:
	INSERT INTO WorkOrder
		(BranchTypeID, DocNumber, CustomerID, CustomerName, ResponsiblePersonID, PatientID, Created, WorkDescription, OfficeAdminID, OfficeAdminName)
	VALUES 
		(2, @docNumber, @customerID, @customerName, @responsiblePersonId, @patientID, GETDATE(), @workDescription, @officeAdminID, @officeAdminName)

	SET @workOrderID = SCOPE_IDENTITY()

	INSERT INTO WorkOrderDL
		(WorkOrderDLID, DoctorFullName, PatientFullName, PatientAge, TransparenceID, FittingDate, DateOfCompletion, ColorAndFeatures)
	VALUES
		(@workOrderID, @doctorFullName, @patientFullName, @patientAge, @transparenceID, @fittingDate, @dateOfCompletion, @colorAndFeatures)

END