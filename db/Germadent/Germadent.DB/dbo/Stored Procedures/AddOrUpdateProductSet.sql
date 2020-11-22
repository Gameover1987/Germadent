-- =============================================
-- Author:		Name
-- Create date: 03.11.2020
-- Description:	Добавление или изменение набора изделий для ценовой позиции
-- =============================================
CREATE PROCEDURE [dbo].[AddOrUpdateProductSet] 
	
	@pricePositionId int,
	@jsonStringProduct nvarchar(max)
	
AS
BEGIN
	
	SET NOCOUNT ON;
	
	BEGIN TRAN
	-- Чистим набор от старого содержимого
	DELETE
	FROM ProductSet
	WHERE PricePositionID = @pricePositionId

	-- Наполняем новым содержимым, распарсив строку json
	INSERT INTO ProductSet
	(PricePositionID, ProductID)
	SELECT PricePositionID = @pricePositionId, ProductID
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

