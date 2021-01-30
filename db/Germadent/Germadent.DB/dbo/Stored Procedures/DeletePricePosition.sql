-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 07.11.2020
-- Description:	Удаление ценовой позиции
-- =============================================
CREATE PROCEDURE DeletePricePosition 
	
	@pricePositionId int, 
	@resultCount int output

AS
BEGIN
	
	SET NOCOUNT ON;

    IF NOT EXISTS (SELECT PricePositionID FROM ToothCard WHERE PricePositionID = @pricePositionId)
			BEGIN
				DELETE
				FROM ProductSet
				WHERE PricePositionID = @pricePositionId
				
				DELETE
				FROM PricePositions
				WHERE PricePositionID = @pricePositionId

				SET @resultCount = @@rowcount
			END

			ELSE BEGIN
				SET @resultCount = 0
			END	

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeletePricePosition] TO [gdl_user]
    AS [dbo];

