-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 30.11.2019
-- Description:	Создание заказ-наряда
-- =============================================
CREATE PROCEDURE [dbo].[AddWorkOrder] 
	
	@branchTypeID int,
	@customerID int = NULL,
	@responsiblePersonId int = NULL,
	@patientFullName nvarchar(150) = NULL,
	@patientGender bit = NULL,
	@patientAge tinyint = NULL,
	@dateComment nvarchar(50) = NULL,
	@prostheticArticul nvarchar(50) = NULL,
	@workDescription nvarchar(250) = NULL,
	@flagStl bit = NULL,
	@flagCashless bit = 1,
	@creatorId int,
	@fittingDate datetime = NULL,
	@dateOfCompletion datetime = NULL,
	@jsonToothCardString varchar(MAX),
	@jsonEquipmentsString varchar(MAX),
	@jsonAttributesString varchar(MAX),
	@workOrderID int output,
	@docNumber nvarchar(10) output,
	@created datetime output
	
AS
BEGIN

	SET NOCOUNT, XACT_ABORT ON;

	-- Чтобы неоправданно не возрастало значение Id в ключевом поле - сначала его "подбивка":
	BEGIN TRAN
		DECLARE @max_Id int
		SELECT @max_Id = ISNULL(MAX(WorkOrderID), 0)
		FROM dbo.WorkOrder

		EXEC dbo.IdentifierAlignment WorkOrder, @max_Id
		REVERT		
	COMMIT

	BEGIN TRAN

		-- Генерируем номер документа, для каждого типа филиала свой:	
		IF @branchTypeID = 1 BEGIN
			SET @docNumber = CONCAT(CAST((NEXT VALUE FOR dbo.SequenceWorkOrderNumber) AS nvarchar(8)), '-MC', '~', YEAR(GETDATE())-2000)
				END
		ELSE IF @branchTypeID = 2 BEGIN
			SET @docNumber = CONCAT(CAST((NEXT VALUE FOR dbo.SequenceWorkOrderNumber) AS nvarchar(8)), '-DL', '~', YEAR(GETDATE())-2000)
				END

		-- Собственно вставка, сначала в основную таблицу:

		INSERT INTO dbo.WorkOrder
			(BranchTypeID,	 DocNumber, CustomerID,	PatientFullName, PatientGender,		PatientAge, ResponsiblePersonID, 	FittingDate, DateOfCompletion,	DateComment, ProstheticArticul,		WorkDescription, FlagStl,  FlagCashless)
		VALUES 
			(@branchTypeID, @docNumber, @customerID, @patientFullName, @patientGender, @patientAge, @responsiblePersonId,  @fittingDate, @dateOfCompletion, @dateComment, @prostheticArticul, @workDescription, @flagStl, @flagCashless)

		SET @workOrderID = SCOPE_IDENTITY()
		
		-- Затем - в подчинённые:
		EXEC dbo.AddOrUpdateToothCardInWO @workOrderID, @jsonToothCardString
		EXEC dbo.AddOrUpdateAdditionalEquipmentInWO @workOrderID, @jsonEquipmentsString
		EXEC dbo.AddOrUpdateAttributesSet @workOrderID, @jsonAttributesString
		EXEC dbo.ChangeStatusWorkOrder @workOrderId, 0, @creatorId, '', @created
	
	COMMIT
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddWorkOrder] TO [gdl_user]
    AS [dbo];

