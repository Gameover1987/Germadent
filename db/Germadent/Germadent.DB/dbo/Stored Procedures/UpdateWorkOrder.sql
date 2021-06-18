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
	, @urgencyRatio float
	, @patientFullName nvarchar(150)
	, @patientGender bit
	, @patientAge tinyint	
	, @fittingDate datetime
	, @dateOfCompletion datetime
	, @jsonToothCardString varchar(MAX)
	, @jsonEquipmentsString varchar(MAX)
	, @jsonAttributesString varchar(MAX)
	, @created datetime output
	
AS
BEGIN

	SET NOCOUNT, XACT_ABORT ON;

	DECLARE
	@currentStatusWO int

	-- Определяем текущий статус заказ-наряда
	SELECT @currentStatusWO = Status
	FROM dbo.StatusList
	WHERE WorkOrderID = @workOrderId AND StatusChangeDateTime = (SELECT MAX(StatusChangeDateTime)
																	FROM dbo.StatusList
																	WHERE WorkOrderID = @workOrderId)

	-- Если заказ-наряд уже закрыт - никаких дальнейших действий
	IF @currentStatusWO = 100
		RETURN
	
	ELSE BEGIN
	BEGIN TRAN
	
		UPDATE dbo.WorkOrder
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
			, UrgencyRatio = ROUND(@urgencyRatio, 2)
			, FlagWorkAccept = @flagWorkAccept
			, FlagStl = @flagStl
			, FlagCashless = @flagCashless

		WHERE WorkOrderID = @workOrderID
		
		EXEC dbo.AddOrUpdateToothCardInWO @workOrderID, @jsonToothCardString
		EXEC dbo.AddOrUpdateAdditionalEquipmentInWO @workOrderID, @jsonEquipmentsString
		EXEC dbo.AddOrUpdateAttributesSet @workOrderID, @jsonAttributesString


	COMMIT

	-- Напоминаем программе дату и время создания заказ-наряда
	SELECT @created = StatusChangeDateTime FROM dbo.StatusList WHERE WorkOrderID = @workOrderID AND Status = 0
	END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateWorkOrder] TO [gdl_user]
    AS [dbo];

