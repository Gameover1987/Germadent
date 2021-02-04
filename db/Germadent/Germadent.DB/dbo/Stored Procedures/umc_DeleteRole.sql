-- =============================================
-- Author:		Alexey Kiolosenok
-- Create date: 19.10.2020
-- Description:	Каскадное удаление выбранной роли
-- =============================================
CREATE PROCEDURE [dbo].[umc_DeleteRole] 
	
	@roleId int
	  
AS
BEGIN
	
	SET NOCOUNT ON;

    	DELETE 
	FROM dbo.Roles
	WHERE RoleID = @roleId

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[umc_DeleteRole] TO [gdl_user]
    AS [dbo];

