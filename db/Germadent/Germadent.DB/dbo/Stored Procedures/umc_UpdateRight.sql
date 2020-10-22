-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 21.10.2020
-- Description:	Редактирование права
-- =============================================
CREATE PROCEDURE umc_UpdateRight 
	
	@rightId int, 
	@rightDescription nvarchar(MAX),
	@isObsolete bit

AS
BEGIN
	
	SET NOCOUNT ON;

    UPDATE Rights
	SET RightDescription = @rightDescription,
		IsObsolete = @isObsolete
	WHERE RightID = @rightId

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[umc_UpdateRight] TO [gdl_user]
    AS [dbo];

