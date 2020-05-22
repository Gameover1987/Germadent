-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 30.11.2019
-- Description:	Создание заказ-наряда зуботехнической лаборатории
-- =============================================
CREATE PROCEDURE [dbo].[AddNewWorkOrder] 
	
	@branchTypeID int = 2,
	@customerID int = NULL,
	@responsiblePersonId int = NULL,
	@patientFullName nvarchar(150) = NULL,
	@patientGender bit = NULL,
	@patientAge tinyint = NULL,
	@dateDelivery datetime = NULL,
	@dateComment nvarchar(50) = NULL,
	@prostheticArticul nvarchar(50) = NULL,
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
	BEGIN
		DECLARE @max_Id int
		SELECT @max_Id = ISNULL(MAX(WorkOrderID), 0)
		FROM WorkOrder

		EXEC IdentifierAlignment WorkOrder, @max_Id
		REVERT		
	END

	-- Генерируем номер документа для каждого филиалов свой:	
	IF @branchTypeID = 1 BEGIN
		SET @docNumber = CONCAT(CAST((NEXT VALUE FOR dbo.SequenceNumberMC) AS nvarchar(6)), '-MC', '~', YEAR(GETDATE())-2000)
			END
	ELSE BEGIN
		SET @docNumber = CONCAT(CAST((NEXT VALUE FOR dbo.SequenceNumberDL) AS nvarchar(6)), '-DL', '~', YEAR(GETDATE())-2000)
			END

	-- Собственно вставка:
	INSERT INTO WorkOrder
		(BranchTypeID,	 DocNumber, CustomerID,	PatientFullName, PatientGender,		PatientAge, ResponsiblePersonID, Created,	FittingDate, DateOfCompletion,	DateDelivery,	DateComment, ProstheticArticul,		WorkDescription, OfficeAdminID, OfficeAdminName)
	VALUES 
		(@branchTypeID, @docNumber, @customerID, @patientFullName, @patientGender, @patientAge, @responsiblePersonId, GETDATE(), @fittingDate, @dateOfCompletion, @dateDelivery, @dateComment, @prostheticArticul, @workDescription, @officeAdminID, @officeAdminName)

	SET @workOrderID = SCOPE_IDENTITY()

	INSERT INTO WorkOrderDL
		(WorkOrderDLID, TransparenceID, ColorAndFeatures)
	VALUES
		(@workOrderID, @transparenceID, @colorAndFeatures)

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddNewWorkOrder] TO [gdl_user]
    AS [dbo];

