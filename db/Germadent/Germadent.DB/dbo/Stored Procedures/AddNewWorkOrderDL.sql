﻿-- =============================================
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
		SELECT @max_Id = MAX(WorkOrderID)
		FROM WorkOrder

		EXEC IdentifierAlignment WorkOrder, @max_Id
		REVERT		
	END

	-- Генерируем номер документа для каждого филиалов свой:	
	SET @docNumber = CONCAT(CAST((NEXT VALUE FOR dbo.SequenceNumberDL) AS nvarchar(6)), '-ЗТЛ')
	
	-- Получение id клиента. Если клиента ещё нет - создаём его в таблице
	SET @customerID = (SELECT CustomerID FROM Customers WHERE CustomerName = @customerName)
	IF @customerID IS NULL EXEC AddNewCustomer @customerName, @customerID output

	-- Получение id доктора
	SET @responsiblePersonId = (SELECT ResponsiblePersonID FROM ResponsiblePersons WHERE ResponsiblePerson = @doctorFullName)
	IF @responsiblePersonId IS NULL EXEC AddNewRespPerson @customerID, 'Доктор', @doctorFullName, NULL, @responsiblePersonId output

	-- Собственно вставка:
	INSERT INTO WorkOrder
		(BranchTypeID, DocNumber, CustomerID, CustomerName, PatientFullName, ResponsiblePersonID, PatientID, Created, ProstheticArticul, WorkDescription, OfficeAdminID, OfficeAdminName)
	VALUES 
		(2, @docNumber, @customerID, @customerName, @patientFullName, @responsiblePersonId, @patientID, GETDATE(), @prostheticArticul, @workDescription, @officeAdminID, @officeAdminName)

	SET @workOrderID = SCOPE_IDENTITY()

	INSERT INTO WorkOrderDL
		(WorkOrderDLID, DoctorFullName, PatientAge, TransparenceID, FittingDate, DateOfCompletion, ColorAndFeatures)
	VALUES
		(@workOrderID, @doctorFullName, @patientAge, @transparenceID, @fittingDate, @dateOfCompletion, @colorAndFeatures)

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddNewWorkOrderDL] TO [gdl_user]
    AS [dbo];

