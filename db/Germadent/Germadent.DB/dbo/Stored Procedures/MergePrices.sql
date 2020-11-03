-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 03.11.2020
-- Description:	Добавление / изменение цен
-- =============================================
CREATE PROCEDURE [dbo].[MergePrices] 
	
	@jsonStringPrices nvarchar(max)
	
AS
BEGIN
	
	SET NOCOUNT ON;

    -- Скармливаем json-строку обобщённому табличному выражению, оно же - таблица-источник:
    WITH src(PricePositionID, DateBegin, PriceSTL, PriceModel)
	AS 
	(
		SELECT PricePositionID, DateBegin, PriceSTL, PriceModel 
			FROM OPENJSON (@jsonStringPrices) 
			WITH (PricePositionId int, DateBegin date, PriceSTL money, PriceModel money)
	) 

	-- Собственно слияние:
	MERGE Prices AS trg
	USING src
	ON (trg.PricePositionID = src.PricePositionID AND trg.DateBegin = src.DateBegin)

	WHEN MATCHED 
		THEN UPDATE
			SET PriceSTL = src.PriceSTL,
				PriceModel = src.PriceModel

	WHEN NOT MATCHED 
		THEN INSERT (PricePositionID, DateBegin, PriceSTL, PriceModel)
		VALUES (src.PricePositionID, GETDATE(), src.PriceSTL, src.PriceModel)

	WHEN NOT MATCHED BY SOURCE
	THEN 
		DELETE;

END