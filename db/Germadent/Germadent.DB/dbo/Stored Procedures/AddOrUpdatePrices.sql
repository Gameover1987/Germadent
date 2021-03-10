-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 20.11.2020
-- Description:	Добавление, редактирование, удаление цен
-- =============================================
CREATE PROCEDURE [dbo].[AddOrUpdatePrices] 
	
	@pricePositionId int,
	@jsonStringPrices nvarchar(MAX) 
	
AS
BEGIN
/*
	declare 
	@pricePositionId int,
	@jsonStringPrices nvarchar(MAX)
	
	set @pricePositionId = 136
	set	@jsonStringPrices = 
		'[
		  {"DateBeginning": "20210301", "PriceStl": 300, "PriceModel": 400},
		  {"DateBeginning": "2020-07-19", "PriceStl": 22, "PriceModel": 44},
		  {"DateBeginning": "2020-08-01", "PriceStl": 100, "PriceModel": 200},
		  {"DateBeginning": "2020-11-22", "PriceStl": null, "PriceModel": 300},
		  {"DateBeginning": "2020-12-01", "PriceStl": 220, "PriceModel": 330},
		  {"DateBeginning": "20210101", "PriceStl": 300, "PriceModel": 400}
		]';
*/
	SET NOCOUNT, XACT_ABORT ON;
	
		BEGIN TRAN	
		--Удаление всех прежних цен:
		DELETE
		FROM dbo.Prices
		WHERE PricePositionID = @pricePositionId

		CREATE TABLE #PricesCollection (RowNumber int, DateBeginning date, PriceSTL money, PriceModel money)
		
		-- Наполняем временную таблицу содержимым строки json, отсортировав и пронумеровав строки:
		INSERT INTO #PricesCollection (RowNumber, DateBeginning, PriceSTL, PriceModel)
		SELECT ROW_NUMBER() OVER(ORDER BY DateBeginning) AS RowNumber, DateBeginning, PriceSTL, PriceModel
					FROM OPENJSON (@jsonStringPrices)
					WITH (DateBeginning date, PriceStl money, PriceModel money)
		
		-- Добавление поля DateEnd как копии поля DateBeginning со смещением вверх и вставка строк в основную таблицу:
		INSERT INTO dbo.Prices
				(PricePositionID, DateBeginning, PriceSTL, PriceModel, DateEnd)
		SELECT PricePositionID = @pricePositionId, prc.DateBeginning, prc.PriceSTL, prc.PriceModel, d.DateBeginning as DateEnd
		FROM #PricesCollection prc
			LEFT JOIN #PricesCollection d ON prc.RowNumber = d.RowNumber - 1

		DROP TABLE #PricesCollection
	
		COMMIT
	
	-- Удаление нулевых цен
	DELETE
	FROM dbo.Prices
	WHERE PriceModel = 0 OR PriceModel IS NULL
		
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddOrUpdatePrices] TO [gdl_user]
    AS [dbo];

