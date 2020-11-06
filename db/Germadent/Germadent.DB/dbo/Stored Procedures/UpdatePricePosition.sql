-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 03.11.2020
-- Description:	Редактирование ценовой позиции
-- =============================================
CREATE PROCEDURE UpdatePricePosition 
	
	@pricePositionId int,	
	@pricePositionCode nvarchar(20),
	@priceGroupId int,
	@pricePositionName nvarchar(max),
	@materialId int,
	@jsonStringProduct nvarchar(max)

AS
BEGIN
	
	SET NOCOUNT ON;

    UPDATE PricePositions
	SET PricePositionCode = @pricePositionCode,
		PriceGroupID = @priceGroupId,
		PricePositionName = @pricePositionName,
		MaterialID = @materialId
	WHERE PricePositionID = @pricePositionId

	EXEC AddOrUpdateProductSet @jsonStringProduct

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdatePricePosition] TO [gdl_user]
    AS [dbo];

