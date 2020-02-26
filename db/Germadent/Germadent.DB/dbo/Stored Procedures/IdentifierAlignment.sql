-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 24.01.2020
-- Description:	Подбивка значений идентификатора
-- =============================================
CREATE PROCEDURE [dbo].[IdentifierAlignment] 
	
	@tableName nvarchar(100),
	@max_Id int

AS
BEGIN
	
	SET NOCOUNT ON;
	    
	DBCC checkident (@tableName, reseed, @max_Id)

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IdentifierAlignment] TO [DbccLauncher]
    AS [dbo];

