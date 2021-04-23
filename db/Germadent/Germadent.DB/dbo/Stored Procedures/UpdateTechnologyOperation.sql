-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 22.04.2021
-- Description:	Редактирование технологической операции
-- =============================================
CREATE PROCEDURE UpdateTechnologyOperation 
	
	@technologyOperationId int,
	@technologyOperationUserCode nvarchar(10),
	@technologyOperationName nvarchar(250),
	@employeePositionID int,
	@isObsoleteTechnologyOperation bit

AS
BEGIN
	
	SET NOCOUNT, XACT_ABORT ON;

	BEGIN TRAN

		UPDATE dbo.TechnologyOperations
		SET TechnologyOperationUserCode = @technologyOperationUserCode,
			TechnologyOperationName = @technologyOperationName,
			EmployeePositionID = @employeePositionID,
			IsObsoleteTechnologyOperation = @isObsoleteTechnologyOperation

	COMMIT

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateTechnologyOperation] TO [gdl_user]
    AS [dbo];

