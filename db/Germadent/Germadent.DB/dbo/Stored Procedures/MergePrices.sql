-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 20.11.2020
-- Description:	Добавление, редактирование, удаление цен
-- =============================================
CREATE PROCEDURE MergePrices 
	
	@jsonStringPrices nvarchar(MAX) 
	
AS
BEGIN
	
	SET NOCOUNT ON;

    -- Скармливаем json-строку обобщённому табличному выражению:
	WITH np(PricePositionID, DateBeginning, PriceSTL, PriceModel, DateEnd)
	AS
	(
		SELECT PricePositionID, DateBeginning, PriceSTL, PriceModel, DateEnd
		FROM OPENJSON (@jsonStringPrices)
		WITH (PricePositionId int, DateBeginning date, PriceSTL money, PriceModel money, DateEnd date)
		
	)

	MERGE Prices AS p
	USING np
	ON (p.PricePositionID = np.PricePositionID)
	WHEN MATCHED
		THEN UPDATE
			SET DateBeginning = np.DateBeginning,
				PriceSTL = np.PriceSTL,
				PriceModel = np.PriceModel,
				DateEnd = np.DateEnd
	WHEN NOT MATCHED
		THEN INSERT(DateBeginning, PriceSTL, PriceModel, DateEnd)
		VALUES (np.DateBeginning, np.PriceSTL, np.PriceModel, np.DateEnd);
		
	--Удаление нулевых цен:
	DELETE
	FROM Prices
	WHERE (PriceSTL = 0 OR PriceSTL IS NULL) 
		AND (PriceModel = 0 OR PriceModel IS NULL)

END