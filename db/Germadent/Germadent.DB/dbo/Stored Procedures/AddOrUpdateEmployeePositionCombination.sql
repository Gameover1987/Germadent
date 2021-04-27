-- =============================================
-- Author: Алексей Колосенок
-- Create date: 25.04.2021
-- Description:	Добавление, редактирование, удаление набора должностей для сотрудника
-- =============================================
CREATE PROCEDURE [dbo].[AddOrUpdateEmployeePositionCombination] 

	@userId int,
	@jsonStringEPC nvarchar(max)
AS
BEGIN
	
	SET NOCOUNT, XACT_ABORT ON;

	BEGIN TRAN
		-- Чистим набор должностей от старого содержимого
		DELETE
		FROM dbo.EmployeePositionsCombination
		WHERE EmployeeID = @userId

		INSERT INTO dbo.EmployeePositionsCombination
		(EmployeeID, EmployeePositionID, QualifyingRank)
		SELECT EmployeeID = @userId, EmployeePositionID, QualifyingRank
		FROM OPENJSON(@jsonStringEPC)
			WITH(EmployeePositionId int, QualifyingRank tinyint)
    
	COMMIT

	-- Удаляем незначащие записи
	DELETE
	FROM dbo.EmployeePositionsCombination
	WHERE EmployeePositionID IS NULL

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddOrUpdateEmployeePositionCombination] TO [gdl_user]
    AS [dbo];

