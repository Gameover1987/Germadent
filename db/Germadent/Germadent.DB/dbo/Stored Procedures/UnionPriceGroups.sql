﻿-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 03.11.2020
-- Description:	Объединение/удаление ценовых групп
-- =============================================
CREATE PROCEDURE UnionPriceGroups 
	
	@oldPriceGroupId int,
	@newPriceGroupId int,
	@resultCount int output

AS
BEGIN
	
	SET NOCOUNT ON;

    IF @newPriceGroupId IS NOT NULL 
		BEGIN
			UPDATE PricePositions 
			SET PriceGroupID = @newPriceGroupId 
			WHERE PriceGroupID = @oldPriceGroupId
	
			DELETE
			FROM PriceGroups
			WHERE PriceGroupID = @oldPriceGroupId

			SET @resultCount = @@rowcount
		END
		ELSE IF @newPriceGroupId IS NULL AND NOT EXISTS (SELECT PriceGroupID FROM PricePositions WHERE PriceGroupID = @oldPriceGroupId)
			BEGIN
				DELETE
				FROM PriceGroups
				WHERE PriceGroupID = @oldPriceGroupId

				SET @resultCount = @@rowcount
			END

			ELSE BEGIN
				SET @resultCount = 0
			END	

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnionPriceGroups] TO [gdl_user]
    AS [dbo];

