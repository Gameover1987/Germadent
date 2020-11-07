-- =============================================
-- Author:		Name
-- Create date: 03.11.2020
-- Description:	Добавление или изменение набора изделий для ценовой группы
-- =============================================
CREATE PROCEDURE AddOrUpdateProductSet 
	
	@jsonStringProduct nvarchar(max)
	
AS
BEGIN
	
	SET NOCOUNT ON;

    -- Достаём нужный id
	DECLARE @pricePositionId int

	SET @pricePositionId = (SELECT DISTINCT PricePositionID
								FROM OPENJSON (@jsonStringProduct)
								WITH (PricePositionId int))

	-- Чистим набор от старого содержимого
	DELETE
	FROM ProductSet
	WHERE PricePositionID = @pricePositionId

	-- Наполняем новым содержимым, распарсив строку json
	INSERT INTO ProductSet
	(PricePositionID, ProductID)
	SELECT PricePositionID, ProductID
	FROM OPENJSON (@jsonStringProduct)
	WITH (PricePositionId int, ProductId int)

	-- Удаляем незначащие записи
	DELETE
	FROM ProductSet
	WHERE ProductID = 0 OR ProductID IS NULL

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddOrUpdateProductSet] TO [gdl_user]
    AS [dbo];

