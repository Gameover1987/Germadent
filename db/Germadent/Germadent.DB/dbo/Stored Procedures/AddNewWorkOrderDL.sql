-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 30.11.2019
-- Description:	Создание заказ-наряда зуботехнической лаборатории
-- =============================================
CREATE PROCEDURE [dbo].[AddNewWorkOrderDL] 

	@docNumber nvarchar(10),
	@customerID int,
	@responsiblePersonId int,
	@patientID int,
	@workDescription nvarchar(250) = NULL,
	@officeAdminID int,
	@transparenceID int = NULL,
	@fittingDate datetime = NULL,
	@dateOfCompletion datetime = NULL,
	@colorAndFeatures nvarchar(100) = NULL,
	@workOrderID int output

AS
BEGIN

	

	-- Чтобы неоправданно не возрастало значение Id в ключевом поле - сначала его "подбивка":
	DECLARE @max_Id int
	SELECT @max_Id = MAX(WorkOrderID)
	FROM WorkOrder
	DBCC checkident (WorkOrder, reseed, @max_Id)

	-- Собственно вставка:
	INSERT INTO WorkOrder
		(BranchTypeID, DocNumber, ResponsiblePersonID, CustomerID, PatientID, Created, WorkDescription, OfficeAdminID)
	VALUES 
		(2, @docNumber, @customerID, @responsiblePersonId, @patientID, GETDATE(), @workDescription, @officeAdminID)

	SET @workOrderID = SCOPE_IDENTITY()

	INSERT INTO WorkOrderDL
		(WorkOrderDLID, TransparenceID, FittingDate, DateOfCompletion, ColorAndFeatures)
	VALUES
		(@workOrderID, @transparenceID, @fittingDate, @dateOfCompletion, @colorAndFeatures)

END