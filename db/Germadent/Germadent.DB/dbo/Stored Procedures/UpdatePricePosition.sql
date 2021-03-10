-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 03.11.2020
-- Description:	Редактирование ценовой позиции
-- =============================================
CREATE PROCEDURE [dbo].[UpdatePricePosition] 
	
	@pricePositionId int,	
	@pricePositionCode nvarchar(20),
	@priceGroupId int,
	@pricePositionName nvarchar(max),
	@materialId int,
	@jsonStringProduct nvarchar(max),
	@jsonStringPrices nvarchar(max)

AS
BEGIN
	
	SET NOCOUNT, XACT_ABORT ON;

	BEGIN TRAN

		UPDATE dbo.PricePositions
		SET PricePositionCode = @pricePositionCode,
			PriceGroupID = @priceGroupId,
			PricePositionName = @pricePositionName,
			MaterialID = @materialId
		WHERE PricePositionID = @pricePositionId

		EXEC dbo.AddOrUpdateProductSet @pricePositionId, @jsonStringProduct

		EXEC dbo.AddOrUpdatePrices @pricePositionId, @jsonStringPrices

	COMMIT

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdatePricePosition] TO [gdl_user]
    AS [dbo];

