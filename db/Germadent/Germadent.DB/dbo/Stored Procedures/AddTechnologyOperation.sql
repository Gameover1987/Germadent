-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 22.04.2021
-- Description:	Добавление технологической операции
-- =============================================
CREATE PROCEDURE AddTechnologyOperation 
	
	@technologyOperationUserCode nvarchar(10) = null,
	@technologyOperationName nvarchar(250),
	@employeePositionID int,
	@isObsoleteTechnologyOperation bit = null,
	@technologyOperationId int output

AS
BEGIN
	
	SET NOCOUNT, XACT_ABORT ON;

    -- Чтобы неоправданно не возрастало значение Id в ключевом поле - сначала его "подбивка":
	BEGIN TRAN
		DECLARE @max_Id int
		SELECT @max_Id = ISNULL(MAX(TechnologyOperationID), 0)
		FROM dbo.TechnologyOperations

		EXEC dbo.IdentifierAlignment TechnologyOperations, @max_Id
	
		REVERT
	COMMIT

	BEGIN TRAN
		-- Собственно вставка, сначала - в основную таблицу:
		INSERT INTO dbo.TechnologyOperations
		(TechnologyOperationUserCode, TechnologyOperationName, EmployeePositionID, IsObsoleteTechnologyOperation)
		VALUES
		(@technologyOperationUserCode, @technologyOperationName, @employeePositionID, @isObsoleteTechnologyOperation)

		SET @technologyOperationId = SCOPE_IDENTITY()

	COMMIT

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddTechnologyOperation] TO [gdl_user]
    AS [dbo];

