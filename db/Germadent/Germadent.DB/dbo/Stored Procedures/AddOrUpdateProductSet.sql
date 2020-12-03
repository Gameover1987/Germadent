-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 03.11.2020
-- Description:	Добавление или изменение набора изделий для ценовой позиции
-- =============================================
CREATE PROCEDURE [dbo].[AddOrUpdateProductSet] 
	
	@pricePositionId int,
	@jsonStringProduct nvarchar(max)
	
AS
BEGIN
	
	SET NOCOUNT ON
	SET XACT_ABORT ON;
	/*
	declare 
		@pricePositionId int,
		@jsonStringProduct nvarchar(max)

		set @pricePositionId = 137
		set @jsonStringProduct = 
		'[
		 {"PricePositionId": null, "ProductId": 2},
		 {"PricePositionId": null, "ProductId": 14},
		 {"PricePositionId": null, "ProductId": 18}
		]';
	*/
	BEGIN TRAN
		-- Чистим набор от старого содержимого
		DELETE
		FROM ProductSet
		WHERE PricePositionID = @pricePositionId

		-- Наполняем новым содержимым, распарсив строку json
		INSERT INTO ProductSet
		(PricePositionID, ProductID)
		SELECT PricePositionID = @pricePositionId, ProductId
		FROM OPENJSON (@jsonStringProduct)
		WITH (ProductId int)

	COMMIT

	-- Удаляем незначащие записи
	DELETE
	FROM ProductSet
	WHERE ProductID = 0 OR ProductID IS NULL

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddOrUpdateProductSet] TO [gdl_user]
    AS [dbo];

