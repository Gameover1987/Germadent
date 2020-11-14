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

	DECLARE @maxDateBeginning date

	SELECT @maxDateBeginning = MAX(DateBeginning) FROM Prices WHERE PricePositionID = @pricePositionId

	IF @maxDateBeginning IS NULL BEGIN
		SET @maxDateBeginning = '17530101'
	END

	IF @dateBeginning > @maxDateBeginning BEGIN

		UPDATE Prices
		SET DateEnd = @dateBeginning
		WHERE PricePositionID = @pricePositionId

		INSERT INTO Prices
		(PricePositionID, DateBeginning, PriceSTL, PriceModel)
		VALUES
		(@pricePositionId, @dateBeginning, @priceSTL, @priceModel)
	
	END
	
	ELSE 
		PRINT CONCAT('На эту дату уже есть актуальная цена, выберите другую дату, позднее ', @maxDateBeginning)
	

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddPrice] TO [gdl_user]
    AS [dbo];

