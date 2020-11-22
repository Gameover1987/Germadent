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
		  {"DateBeginning": "2020-07-19", "PriceSTL": 22, "PriceModel": 44, "DateEnd": "2020-08-01"},
		  {"DateBeginning": "2020-08-01", "PriceSTL": 100, "PriceModel": 200, "DateEnd": "2020-11-22"},
		  {"DateBeginning": "2020-11-22", "PriceSTL": 200, "PriceModel": 300, "DateEnd": "2020-12-01"},
		  {"DateBeginning": "2020-12-01", "PriceSTL": 220, "PriceModel": 330, "DateEnd": "2021-01-01"},
		  {"DateBeginning": "20210101", "PriceSTL": 300, "PriceModel": 400, "DateEnd": null}
		]';
*/
	SET NOCOUNT ON;
	BEGIN TRAN	
	--Удаление всех прежних цен:
	DELETE
	FROM Prices
	WHERE PricePositionID = @pricePositionId

	-- Наполняем новым содержимым, распарсив строку json:
	INSERT INTO Prices
	(PricePositionID, DateBeginning, PriceSTL, PriceModel, DateEnd)
	SELECT PricePositionID = @pricePositionId, DateBeginning, PriceSTL, PriceModel, DateEnd
		FROM OPENJSON (@jsonStringPrices)
		WITH (DateBeginning date, PriceSTL money, PriceModel money, DateEnd date)
	
	COMMIT

	DELETE
	FROM Prices
	WHERE PriceModel = 0 OR PriceModel IS NULL
		
END