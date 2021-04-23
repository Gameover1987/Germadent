-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 22.04.2021
-- Description:	Добавление, редактирование, удаление расценок
-- =============================================
CREATE PROCEDURE AddOrUpdateRates 
	
	@technologyOperationId int, 
	@jsonStringRates nvarchar(MAX)

AS
BEGIN
	
	SET NOCOUNT, XACT_ABORT ON;

		BEGIN TRAN
			--Удаление всех прежних расценок:
		DELETE
		FROM dbo.Rates
		WHERE TechnologyOperationID = @technologyOperationId

		CREATE TABLE #RatesCollection (RowNumber int, QualifyingRank tinyint, Rate money, DateBeginning date)

		-- Наполняем временную таблицу содержимым строки json, отсортировав и пронумеровав строки:
		INSERT INTO #RatesCollection (RowNumber, QualifyingRank, Rate, DateBeginning)
		SELECT ROW_NUMBER() OVER(ORDER BY QualifyingRank, DateBeginning) AS RowNumber, QualifyingRank, Rate, DateBeginning
			FROM OPENJSON (@jsonStringRates)
			WITH (QualifyingRank tinyint, Rate money, DateBeginning date)

		-- Добавление поля DateEnd как копии поля DateBeginning со смещением вверх и вставка строк в основную таблицу:
		INSERT INTO dbo.Rates
			(TechnologyOperationID, QualifyingRank, Rate, DateBeginning, DateEnd)
			SELECT TechnologyOperationID = @technologyOperationId, rc.QualifyingRank, rc.Rate, rc.DateBeginning, rca.DateBeginning AS DateEnd
			FROM #RatesCollection rc
				LEFT JOIN #RatesCollection rca ON rc.RowNumber = rca.RowNumber - 1

			DROP TABLE #RatesCollection

		COMMIT

		-- Удаление нулевых цен
		DELETE
		FROM dbo.Rates
		WHERE Rate = 0 OR Rate IS NULL

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddOrUpdateRates] TO [gdl_user]
    AS [dbo];

