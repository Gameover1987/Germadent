-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 16.12.2019
-- Description:	Внесение изменений в заказ-наряд
-- =============================================
CREATE PROCEDURE [dbo].[UpdateWorkOrder]
	
	@branchTypeID int
	, @workOrderID int
	, @docNumber nvarchar(10)
	, @customerID int
	, @responsiblePersonId int
	, @flagWorkAccept bit
	, @dateComment nvarchar(50)
	, @prostheticArticul nvarchar(50)
	, @workDescription nvarchar(250)
--	, @officeAdminID int
	, @officeAdminName nvarchar(50)
	, @patientFullName nvarchar(150)
	, @patientGender bit
	, @patientAge tinyint	
	, @fittingDate datetime
	, @dateOfCompletion datetime
	, @additionalInfo nvarchar(70)
	, @carcassColor nvarchar(30)
	, @implantSystem nvarchar(70)
	, @individualAbutmentProcessing nvarchar(70)
	, @understaff nvarchar(100)
	, @transparenceID int
	, @colorAndFeatures nvarchar(100)
	
AS
BEGIN
	-- Никаких изменений, если заказ-наряд закрыт
	IF((SELECT Status FROM WorkOrder WHERE WorkOrderID = @workOrderID) = 9)
		BEGIN
			RETURN
		END

	-- Изменение основной таблицы
	UPDATE WorkOrder
	SET  DocNumber = @docNumber
		, CustomerID = @customerID
		, PatientFullName = @patientFullName
		, PatientGender = @patientGender
		, PatientAge = @patientAge
		, ResponsiblePersonID = @responsiblePersonId
		, FittingDate = @fittingDate
		, DateOfCompletion = @dateOfCompletion
		, DateComment = @dateComment
		, ProstheticArticul = @prostheticArticul
		, WorkDescription = @workDescription
		, FlagWorkAccept = @flagWorkAccept
--		, OfficeAdminID = @officeAdminID
		, OfficeAdminName = @officeAdminName	
	WHERE WorkOrderID = @workOrderID
	
	-- Изменение подчинённых таблиц в зависимости от типа филиала
	IF @branchTypeID = 1 BEGIN
		UPDATE WorkOrderMC
		SET AdditionalInfo = @additionalInfo
			, CarcassColor = @carcassColor
			, ImplantSystem = @implantSystem
			, IndividualAbutmentProcessing = @individualAbutmentProcessing
			, Understaff = @understaff
		WHERE WorkOrderMCID = @workOrderID
	END
	ELSE IF @branchTypeID = 2 BEGIN
		UPDATE WorkOrderDL
		SET TransparenceID = @transparenceID		
			, ColorAndFeatures = @colorAndFeatures
		WHERE WorkOrderDLID = @workOrderID
	END

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateWorkOrder] TO [gdl_user]
    AS [dbo];

