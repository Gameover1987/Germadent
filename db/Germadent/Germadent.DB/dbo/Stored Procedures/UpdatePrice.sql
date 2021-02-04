-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 07.11.2020
-- Description:	Редактирование цены
-- =============================================
CREATE PROCEDURE [dbo].[UpdatePrice] 
	
	@pricePositionId int,
	@dateBeginningCurrent date,
	
	@priceSTL money,
	@priceModel money,	
	@dateEnd date

AS
BEGIN
	
	SET NOCOUNT, XACT_ABORT ON;
	SET ANSI_WARNINGS OFF;

	DECLARE @dateBeginningNew date, -- перетащить во входные параметры
			@maxDateBeginning date,
			@maxDateEnd date

	SET @dateBeginningNew = @dateBeginningCurrent -- пока так
	
	IF @dateEnd IS NULL SET @dateEnd = '99991231'

	-- Редактирование цены с проверкой корректности дат начала и окончания действия
	BEGIN TRAN
		
		DELETE
		FROM dbo.Prices
		WHERE PricePositionID = @pricePositionId
			AND DateBeginning = @dateBeginningCurrent

		SELECT @maxDateBeginning = MAX(DateBeginning) FROM dbo.Prices WHERE PricePositionID = @pricePositionId
		SELECT @maxDateEnd = MAX(DateEnd) FROM dbo.Prices WHERE PricePositionID = @pricePositionId

		IF @dateBeginningNew <= @maxDateBeginning OR @dateEnd <= @maxDateEnd OR @dateBeginningNew > @dateEnd BEGIN
		ROLLBACK
		PRINT 'Даты начала или окончания цены меньше тех, которые уже есть в базе'
		RETURN
		END

		ELSE BEGIN
			INSERT INTO dbo.Prices
			(PricePositionID, DateBeginning, PriceSTL, PriceModel, DateEnd)
			VALUES
			(@pricePositionId, @dateBeginningNew, @priceSTL, @priceModel, @dateEnd)

			UPDATE dbo.Prices
			SET DateEnd = @dateBeginningNew
			WHERE PricePositionID = @pricePositionId
				AND DateEnd = @maxDateEnd

			COMMIT TRAN

		END

		-- Удаление записи с нулевыми ценами и открытие периода актуальности для предыдущих цен
		BEGIN TRAN

			DELETE
			FROM dbo.Prices
			WHERE PricePositionID = @pricePositionId
				AND (PriceSTL = 0 OR PriceSTL IS NULL) 
				AND (PriceModel = 0 OR PriceModel IS NULL)

			SELECT @maxDateBeginning = MAX(DateBeginning) FROM dbo.Prices WHERE PricePositionID = @pricePositionId

			UPDATE dbo.Prices
			SET DateEnd = NULL
			WHERE PricePositionID = @pricePositionId
				AND DateBeginning = @maxDateBeginning

		COMMIT TRAN	

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdatePrice] TO [gdl_user]
    AS [dbo];

