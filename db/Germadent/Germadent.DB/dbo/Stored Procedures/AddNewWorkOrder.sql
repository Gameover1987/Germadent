-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 30.11.2019
-- Description:	Создание заказ-наряда
-- =============================================
CREATE PROCEDURE [dbo].[AddNewWorkOrder] 
	
	@branchTypeID int,
	@customerID int = NULL,
	@responsiblePersonId int = NULL,
	@patientFullName nvarchar(150) = NULL,
	@patientGender bit = NULL,
	@patientAge tinyint = NULL,
	@dateComment nvarchar(50) = NULL,
	@prostheticArticul nvarchar(50) = NULL,
	@workDescription nvarchar(250) = NULL,
	@officeAdminID int = NULL,
	@officeAdminName nvarchar(50) = NULL,	
	@fittingDate datetime = NULL,
	@dateOfCompletion datetime = NULL,
	@additionalInfo nvarchar(70) = NULL,
	@carcassColor nvarchar(30) = NULL,
	@implantSystem nvarchar(70) = NULL,
	@individualAbutmentProcessing nvarchar(70) = NULL,
	@understaff nvarchar(100) = NULL,
	@transparenceID int = NULL,
	@colorAndFeatures nvarchar(100) = NULL,
	@workOrderID int output,
	@docNumber nvarchar(10) output,
	@created datetime output
	
AS
BEGIN


	-- Чтобы неоправданно не возрастало значение Id в ключевом поле - сначала его "подбивка":
	BEGIN
		DECLARE @max_Id int
		SELECT @max_Id = ISNULL(MAX(WorkOrderID), 0)
		FROM WorkOrder

		EXEC IdentifierAlignment WorkOrder, @max_Id
		REVERT		
	END

	-- Генерируем номер документа, для каждого типа филиала свой:	
	IF @branchTypeID = 1 BEGIN
		SET @docNumber = CONCAT(CAST((NEXT VALUE FOR dbo.SequenceWorkOrderNumber) AS nvarchar(8)), '-MC', '~', YEAR(GETDATE())-2000)
			END
	ELSE IF @branchTypeID = 2 BEGIN
		SET @docNumber = CONCAT(CAST((NEXT VALUE FOR dbo.SequenceWorkOrderNumber) AS nvarchar(8)), '-DL', '~', YEAR(GETDATE())-2000)
			END

	-- Собственно вставка, сначала в основную таблицу:
	SET	@created = GETDATE()

	INSERT INTO WorkOrder
		(BranchTypeID,	 DocNumber, CustomerID,	PatientFullName, PatientGender,		PatientAge, ResponsiblePersonID, Created,	FittingDate, DateOfCompletion,	DateComment, ProstheticArticul,		WorkDescription, OfficeAdminID, OfficeAdminName)
	VALUES 
		(@branchTypeID, @docNumber, @customerID, @patientFullName, @patientGender, @patientAge, @responsiblePersonId, @created, @fittingDate, @dateOfCompletion, @dateComment, @prostheticArticul, @workDescription, @officeAdminID, @officeAdminName)

	SET @workOrderID = SCOPE_IDENTITY()

	-- Затем - в подчинённые, для каждого типа филиала - в свою:
	IF @branchTypeID = 1 BEGIN
		INSERT INTO WorkOrderMC
			(WorkOrderMCID, AdditionalInfo, CarcassColor, ImplantSystem, IndividualAbutmentProcessing, Understaff)
		VALUES
			(@workOrderID, @additionalInfo, @carcassColor, @implantSystem, @individualAbutmentProcessing, @understaff)
	END

	ELSE IF @branchTypeID = 2 BEGIN
		INSERT INTO WorkOrderDL
			(WorkOrderDLID, TransparenceID, ColorAndFeatures)
		VALUES
			(@workOrderID, @transparenceID, @colorAndFeatures)
	END

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddNewWorkOrder] TO [gdl_user]
    AS [dbo];

