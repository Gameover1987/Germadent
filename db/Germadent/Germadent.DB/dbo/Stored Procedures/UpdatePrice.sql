-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 07.11.2020
-- Description:	Редактирование цены
-- =============================================
CREATE PROCEDURE [dbo].[UpdatePrice] 
	
	@pricePositionId int,
	@dateBeginningCurrent date,
	@dateBeginningNew date,
	@priceSTL money,
	@priceModel money,	
	@dateEnd date

AS
BEGIN
	
	SET NOCOUNT ON;
	
	UPDATE Prices
	SET DateBeginning = @dateBeginningNew,
		PriceSTL = @priceSTL,
		PriceModel = @priceModel,
		DateEnd = @dateEnd
	WHERE PricePositionID = @pricePositionId
		AND DateBeginning = @dateBeginningCurrent

	DELETE
	FROM Prices
	WHERE PricePositionID = @pricePositionId
		AND (PriceSTL = 0 OR PriceSTL IS NULL) 
		AND (PriceModel = 0 OR PriceModel IS NULL)

END