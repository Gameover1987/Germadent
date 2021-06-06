-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 03.11.2020
-- Description:	Добавление ценовой позиции
-- =============================================
CREATE PROCEDURE [dbo].[AddPricePosition] 
	
	@pricePositionCode nvarchar(20),
	@priceGroupId int,
	@pricePositionName nvarchar(MAX),
	@materialId int,	
	@jsonStringProduct nvarchar(MAX),
--	@jsonStringMaterial nvarchar(MAX),
	@jsonStringPrices nvarchar(MAX),
	@pricePositionId int output
	
AS
BEGIN
	
	SET NOCOUNT, XACT_ABORT ON;

	-- Чтобы неоправданно не возрастало значение Id в ключевом поле - сначала его "подбивка":
	BEGIN TRAN
		DECLARE @max_Id int
		SELECT @max_Id = ISNULL(MAX(PricePositionID), 0)
		FROM dbo.PricePositions

		EXEC dbo.IdentifierAlignment PricePositions, @max_Id
	
		REVERT
	COMMIT
	
	BEGIN TRAN
		-- Собственно вставка, сначала - в основную таблицу:
		INSERT INTO dbo.PricePositions
		(PricePositionCode, PriceGroupID, PricePositionName)
		VALUES
		(@pricePositionCode, @priceGroupId, @pricePositionName)

		SET @pricePositionId = SCOPE_IDENTITY()

		-- Добавление набора изделий:
		EXEC dbo.AddOrUpdateProductSet @pricePositionId, @jsonStringProduct

		-- Добавление набора материалов:
		EXEC dbo.AddOrUpdateMaterialSet @pricePositionId, @materialId --@jsonStringMaterial
	
		-- Добавление цены:
		EXEC dbo.AddOrUpdatePrices @pricePositionId, @jsonStringPrices

	COMMIT
	   
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddPricePosition] TO [gdl_user]
    AS [dbo];

