-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 16.12.2019
-- Description:	Внесение изменений в заказ-наряд
-- =============================================
CREATE PROCEDURE [dbo].[UpdateWorkOrder]
	
	@branchTypeID int
	, @workOrderID int
	, @customerID int
	, @responsiblePersonId int
	, @flagWorkAccept bit
	, @flagStl bit
	, @flagCashless bit
	, @dateComment nvarchar(50)
	, @prostheticArticul nvarchar(50)
	, @workDescription nvarchar(250)
	, @patientFullName nvarchar(150)
	, @patientGender bit
	, @patientAge tinyint	
	, @fittingDate datetime
	, @dateOfCompletion datetime
--	, @jsonToothCardString varchar(MAX),
	, @jsonEquipmentsString varchar(MAX)
	, @jsonAttributesString varchar(MAX)
	, @created datetime output
	
AS
BEGIN

	SET NOCOUNT, XACT_ABORT ON;

	-- Никаких изменений, если заказ-наряд закрыт
	IF((SELECT Status FROM WorkOrder WHERE WorkOrderID = @workOrderID) = 9)
		BEGIN
			RETURN
		END

	BEGIN TRAN
	
		UPDATE WorkOrder
		SET   CustomerID = @customerID
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
			, FlagStl = @flagStl
			, FlagCashless = @flagCashless

		WHERE WorkOrderID = @workOrderID
		
--		EXEC AddOrUpdateToothCardInWO @workOrderID, @jsonToothCardString
		EXEC AddOrUpdateAdditionalEquipmentInWO @workOrderID, @jsonEquipmentsString
		EXEC AddOrUpdateAttributesSet @workOrderID, @jsonAttributesString

		-- Убираем метку о "блокировке" заказ-наряда
		EXEC UserReadingWO @workOrderID

	COMMIT

	-- Напоминаем программе дату и время создания заказ-наряда
	SELECT @created = Created FROM WorkOrder WHERE WorkOrderID = @workOrderID

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateWorkOrder] TO [gdl_user]
    AS [dbo];

