-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 30.11.2019
-- Description:	Создание заказ-наряда зуботехнической лаборатории
-- =============================================
CREATE PROCEDURE [dbo].[AddNewWorkOrderDL] 

	@docNumber nchar(10),
	@customerID int,
	@patientID int,
	@workDescription nvarchar(250) = NULL,
	@officeAdminID int,
	@transparenceID int = NULL,
	@typeOfWork nvarchar(250) = NULL,
	@fittingDate datetime = NULL,
	@dateOfCompletion datetime = NULL,
	@colorAndFeatures nvarchar(100) = NULL

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
		(2, @docNumber, @customerID, @patientID, GETDATE(), @workDescription, @officeAdminID)

	SET @workOrderID = SCOPE_IDENTITY()

	INSERT INTO WorkOrderDL
		(WorkOrderDLID, TransparenceID, TypeOfWork, FittingDate, DateOfCompletion, ColorAndFeatures)
	VALUES
		(@workOrderID, @transparenceID, @typeOfWork, @fittingDate, @dateOfCompletion, @colorAndFeatures)

END