-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 03.11.2020
-- Description:	Редактирование ценовй группы
-- =============================================
CREATE PROCEDURE [dbo].[UpdatePriceGroup] 
	
	@priceGroupId int,
	@branchTypeId int,
	@priceGroupName nvarchar(200)

AS
BEGIN
	
	SET NOCOUNT ON;

	UPDATE dbo.PriceGroups
	SET BranchTypeID = @branchTypeId,
		PriceGroupName = @priceGroupName
	WHERE PriceGroupID = @priceGroupId

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdatePriceGroup] TO [gdl_user]
    AS [dbo];

