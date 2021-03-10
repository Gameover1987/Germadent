-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 21.10.2020
-- Description:	Добавление права
-- =============================================
CREATE PROCEDURE [dbo].[umc_AddRight] 
	
	@applicationId int, 
	@rightName nvarchar(MAX),
	@rightDescription  nvarchar(MAX),
	@rightId int output

AS
BEGIN
	
	SET NOCOUNT ON;

	-- Чтобы неоправданно не возрастало значение Id в ключевом поле - сначала его "подбивка":
	BEGIN
		DECLARE @max_Id int
		SELECT @max_Id = ISNULL(MAX(RightID), 0)
		FROM dbo.Rights

		EXEC IdentifierAlignment Rights, @max_Id
	
		REVERT
	END	   
	
	-- Собственно вставка:
	INSERT INTO dbo.Rights
	(ApplicationID, RightName, RightDescription)
	VALUES
	(@applicationId, @rightName, @rightDescription)

	SET @rightId = SCOPE_IDENTITY()

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[umc_AddRight] TO [gdl_user]
    AS [dbo];

