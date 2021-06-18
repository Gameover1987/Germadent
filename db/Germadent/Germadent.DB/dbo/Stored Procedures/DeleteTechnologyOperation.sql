-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 22.042021
-- Description:	Удаление технологической операции
-- =============================================
CREATE PROCEDURE DeleteTechnologyOperation 
	
	@technologyOperationId int, 
	@resultCount int output

AS
BEGIN
	
	SET NOCOUNT, XACT_ABORT ON;

    IF NOT EXISTS (SELECT TechnologyOperationID FROM dbo.WorkList WHERE TechnologyOperationID = @technologyOperationId) BEGIN
		BEGIN TRAN
			DELETE
			FROM dbo.Rates
			WHERE TechnologyOperationID = @technologyOperationId

			DELETE
			FROM dbo.TechnologyOperations
			WHERE TechnologyOperationID = @technologyOperationId

			SET @resultCount = @@rowcount
		COMMIT
		END

		ELSE 
		SET @resultCount = 0
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteTechnologyOperation] TO [gdl_user]
    AS [dbo];

