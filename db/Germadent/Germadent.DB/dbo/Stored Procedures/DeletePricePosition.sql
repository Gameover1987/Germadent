-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 07.11.2020
-- Description:	Удаление ценовой позиции
-- =============================================
CREATE PROCEDURE [dbo].[DeletePricePosition] 
	
	@pricePositionId int, 
	@resultCount int output

AS
BEGIN
	
	SET NOCOUNT ON;

    IF NOT EXISTS (SELECT PricePositionID FROM dbo.ToothCard WHERE PricePositionID = @pricePositionId)
			BEGIN
				DELETE
				FROM dbo.ProductSet
				WHERE PricePositionID = @pricePositionId
				
				DELETE
				FROM dbo.PricePositions
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

