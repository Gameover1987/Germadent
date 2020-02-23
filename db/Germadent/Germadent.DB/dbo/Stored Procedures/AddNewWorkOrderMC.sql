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
	@patientFullName nvarchar(150) = NULL,
	@dateComment  nvarchar(50) = NULL,
	@prostheticArticul nvarchar(50) = NULL,
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
	BEGIN
		DECLARE @max_Id int
		SELECT @max_Id = MAX(WorkOrderID)
		FROM WorkOrder

		EXEC IdentifierAlignment WorkOrder, @max_Id
		REVERT		
	END

	-- Генерируем номер документа для каждого филиалов свой:
	SET @docNumber = CONCAT(CAST((NEXT VALUE FOR dbo.SequenceNumberMC) AS nvarchar(6)), '-MC', '~', YEAR(GETDATE())-2000)

	-- Получение id клиента. Если клиента ещё нет - создаём его в таблице
	SET @customerID = (SELECT CustomerID FROM Customers WHERE CustomerName = @customerName)
	IF @customerID IS NULL EXEC AddNewCustomer @customerName, @customerID output


	-- Собственно вставка:	
	INSERT INTO WorkOrder
		(BranchTypeID, DocNumber, CustomerID, CustomerName, ResponsiblePersonID, PatientID, PatientFullName, Created, DateComment, ProstheticArticul, WorkDescription, OfficeAdminID, OfficeAdminName)
	VALUES 
		(1, @docNumber, @customerID, @customerName, @responsiblePersonId, @patientID, @patientFullName, GETDATE(), @dateComment, @prostheticArticul, @workDescription, @officeAdminID, @officeAdminName)

	SET @workOrderID = SCOPE_IDENTITY()

	INSERT INTO WorkOrderMC
		(WorkOrderMCID, TechnicFullName, TechnicPhone, AdditionalInfo, CarcassColor, ImplantSystem, IndividualAbutmentProcessing, Understaff)
	VALUES
		(@workOrderID, @technicFullName, @technicPhone, @additionalInfo, @carcassColor, @implantSystem, @individualAbutmentProcessing, @understaff)


END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddNewWorkOrderMC] TO [gdl_user]
    AS [dbo];

