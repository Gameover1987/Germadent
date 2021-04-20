-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 10.03.2021
-- Description:	Добавление или изменение набора материалов для ценовой позиции
-- =============================================
CREATE PROCEDURE [dbo].[AddOrUpdateMaterialSet] 
	
	@pricePositionId int, 
	@jsonStringMaterial nvarchar(max)

AS
BEGIN
	
	SET NOCOUNT, XACT_ABORT ON;
	
	BEGIN TRAN
		
		-- Чистим набор от старого содержимого
		DELETE
		FROM dbo.MaterialSet
		WHERE PricePositionID = @pricePositionId

		-- Наполняем новым содержимым, распарсив строку json
		INSERT INTO dbo.MaterialSet
		(PricePositionID, MaterialID)
		SELECT PricePositionID = @pricePositionId, MaterialId
		FROM OPENJSON (@jsonStringMaterial)
		WITH (MaterialId int)

	COMMIT

	-- Удаляем незначащие записи
	DELETE
	FROM dbo.MaterialSet
	WHERE MaterialID = 0 OR MaterialID IS NULL

END