-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 22.04.2021
-- Description:	Добавление, редактирование, удаление расценок
-- =============================================
CREATE PROCEDURE [dbo].[AddOrUpdateRates] 
	
	@technologyOperationId int, 
	@jsonStringRates nvarchar(MAX)

AS
BEGIN
/*
declare
	@technologyOperationId int, 
	@jsonStringRates nvarchar(MAX)

	set @technologyOperationId = 1
	set @jsonStringRates = 
	'[
	  {"QualifyingRank": "1", "Rate": "120", "DateBeginning": "2021-03-01"},
	  {"QualifyingRank": "1", "Rate": "130", "DateBeginning": "2021-04-01"},
	  {"QualifyingRank": "2", "Rate": "170", "DateBeginning": "2021-03-01"},
	  {"QualifyingRank": "3", "Rate": "220", "DateBeginning": "2021-03-01"},
	  {"QualifyingRank": "2", "Rate": "180", "DateBeginning": "2021-05-01"},
	  {"QualifyingRank": "4", "Rate": "250", "DateBeginning": "2021-03-01"}
	]'
*/	
	SET NOCOUNT, XACT_ABORT ON;

		BEGIN TRAN
			--Удаление всех прежних расценок:
		DELETE
		FROM dbo.Rates
		WHERE TechnologyOperationID = @technologyOperationId

		DECLARE @quantityRanks int

		CREATE TABLE #RatesCollection (QualifyingRank tinyint, Rate money, DateBeginning date)

		-- Наполняем временную таблицу содержимым строки json, отсортировав и пронумеровав строки:
		INSERT INTO #RatesCollection (QualifyingRank, Rate, DateBeginning)
		SELECT QualifyingRank, Rate, DateBeginning
			FROM OPENJSON (@jsonStringRates)
			WITH (QualifyingRank tinyint, Rate money, DateBeginning date)

		-- Считаем количество квалификационных разрядов
		SELECT @quantityRanks = COUNT(DISTINCT QualifyingRank) FROM #RatesCollection

		-- Для каждого квалификационного разряда вставляем строки в основную таблицу с добавлением поля DateEnd как копии поля DateBeginning со смещением вверх
		WHILE @quantityRanks >= 1
		BEGIN
			CREATE TABLE #RatesCollectionPart (RowNumber int, QualifyingRank tinyint, Rate money, DateBeginning date)

			INSERT INTO #RatesCollectionPart (RowNumber, QualifyingRank, Rate, DateBeginning)
			SELECT ROW_NUMBER() OVER(ORDER BY DateBeginning) AS RowNumber, QualifyingRank, Rate, DateBeginning
			FROM #RatesCollection
			WHERE QualifyingRank = @quantityRanks

			INSERT INTO dbo.Rates
			(TechnologyOperationID, QualifyingRank, Rate, DateBeginning, DateEnd)
			SELECT TechnologyOperationID = @technologyOperationId, rcp.QualifyingRank, rcp.Rate, rcp.DateBeginning, rcpa.DateBeginning AS DateEnd
			FROM #RatesCollectionPart rcp LEFT JOIN #RatesCollectionPart rcpa ON rcp.RowNumber = rcpa.RowNumber - 1

			DROP TABLE #RatesCollectionPart

			SET @quantityRanks = @quantityRanks - 1
			
		END

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

