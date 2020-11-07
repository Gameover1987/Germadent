-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 07.11.2020
-- Description:	Добавление цены
-- =============================================
CREATE PROCEDURE [dbo].[AddPrice] 
	
	@pricePositionId int,
	@dateBeginning date = NULL,
	@priceSTL money,
	@priceModel money
	

AS
BEGIN
	
	SET NOCOUNT ON;

	IF @dateBeginning IS NULL BEGIN
		SET @dateBeginning = GETDATE()
	END

	IF @dateBeginning > (SELECT MAX(DateBeginning) FROM Prices WHERE PricePositionID = @pricePositionId) BEGIN

    UPDATE Prices
	SET DateEnd = @dateBeginning
	WHERE PricePositionID = @pricePositionId

	INSERT INTO Prices
	(PricePositionID, DateBeginning, PriceSTL, PriceModel)
	VALUES
	(@pricePositionId, @dateBeginning, @priceSTL, @priceModel)
	
	END
	ELSE BEGIN
	PRINT 'На эту дату уже есть актуальная цена'
	END

END