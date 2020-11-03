-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 03.11.2020
-- Description:	Добавление ценовой группы
-- =============================================
CREATE PROCEDURE AddPriceGroup 
	
	@branchTypeId int,
	@priceGroupName nvarchar(200), 
	@priceGroupId int output

AS
BEGIN
	
	SET NOCOUNT ON;

	-- Чтобы неоправданно не возрастало значение Id в ключевом поле - сначала его "подбивка":
	BEGIN
		DECLARE @max_Id int
		SELECT @max_Id = ISNULL(MAX(PriceGroupID), 0)
		FROM PriceGroups

		EXEC IdentifierAlignment PriceGroups, @max_Id
	
		REVERT
	END
	
	-- Собственно вставка:
	INSERT INTO PriceGroups
	(PriceGroupName, BranchTypeID)
	VALUES
	(@priceGroupName, @branchTypeId)

	SET @priceGroupId = SCOPE_IDENTITY()
    
END