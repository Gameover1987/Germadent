-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 03.11.2020
-- Description:	Добавление ценовой позиции
-- =============================================
CREATE PROCEDURE [dbo].[AddPricePosition] 
	
	@pricePositionCode nvarchar(20),
	@priceGroupId int,
	@pricePositionName nvarchar(max),
	@materialId int,
	@dateBeginning date = NULL,
	@priceSTL money,
	@priceModel money,
	@pricePositionId int output
	
AS
BEGIN
	
	SET NOCOUNT ON;

	-- Чтобы неоправданно не возрастало значение Id в ключевом поле - сначала его "подбивка":
	BEGIN
		DECLARE @max_Id int
		SELECT @max_Id = ISNULL(MAX(PricePositionID), 0)
		FROM PricePositions

		EXEC IdentifierAlignment PricePositions, @max_Id
	
		REVERT
	END
	
	-- Собственно вставка, сначала - в основную таблицу:
	INSERT INTO PricePositions
	(PricePositionCode, PriceGroupID, PricePositionName, MaterialID)
	VALUES
	(@pricePositionCode, @priceGroupId, @pricePositionName, @materialId)

	SET @pricePositionId = SCOPE_IDENTITY()

	-- Добавление цены:
	EXEC AddPrice @pricePositionId, @dateBeginning, @priceSTL, @priceModel
	   
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddPricePosition] TO [gdl_user]
    AS [dbo];

